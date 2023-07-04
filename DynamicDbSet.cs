using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace DynamicEfCore
{
    public class DynamicDbSet : IDynamicDbSet
    {
        public object DbSet { get; private set; }
        public Type EntityType { get; private set; }


        public DynamicDbSet(object dbSet)
        {
            this.DbSet = dbSet;
            var entityTypeName = this.DbSet.GetType().GenericTypeArguments[0].FullName;
            this.EntityType = Assembly.GetExecutingAssembly().GetType(entityTypeName);
        }

        public IDynamicDbSet GetDynamicDbSet(DbContext context, string dbSetName)
        {
            var propInfo = context.GetType().GetProperty(dbSetName);

            var dbSet = propInfo.GetValue(context, null);

            if (dbSet == null)
            {
                throw new Exception("DbSet does not exists on the context");
            }

            return new DynamicDbSet(dbSet);
        }

        public object ToList()
        {
            var toList = typeof(Enumerable).GetMethod("ToList");

            var construtedToList = toList.MakeGenericMethod(this.EntityType);

            return construtedToList.Invoke(null, new[] { this.DbSet });
        }

        public object SearchByTerm(string searchTerm, string targetProperty)
        {
            // create expression tree
            var paramExpr = Expression.Parameter(this.EntityType, "entity");
            var propertyExpr = Expression.Property(paramExpr, targetProperty);
            var containsMethodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var searchTermConstantExpr = Expression.Constant(searchTerm, typeof(string));
            var callExpr = Expression.Call(propertyExpr, containsMethodInfo, searchTermConstantExpr);
            var expressionTree = Expression.Lambda(callExpr, paramExpr);

            // invoke the where methode to search the entities
            var whereMethod = typeof(Queryable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .ToList()
                .FirstOrDefault(mi => mi.Name == "Where")
                .MakeGenericMethod(new[] { this.EntityType });

            var queryable = whereMethod.Invoke(null, new[] { this.DbSet, expressionTree });

            // call to list on the queryable and return the result
            var toList = typeof(Enumerable).GetMethod("ToList");
            var construtedToList = toList.MakeGenericMethod(this.EntityType);
            return construtedToList.Invoke(null, new[] { queryable });
        }
    }
}
