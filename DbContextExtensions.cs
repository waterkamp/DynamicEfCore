using Microsoft.EntityFrameworkCore;

namespace DynamicEfCore
{
    public static class DbContextExtensions
    {
        public static IDynamicDbSet GetDynamicDbSet(this DbContext context, string dbSetName)
        {
            var propInfo = context.GetType().GetProperty(dbSetName);

            var dbSet = propInfo.GetValue(context, null);

            return dbSet == null ? throw new Exception("DbSet does not exists on the context") : (IDynamicDbSet)new DynamicDbSet(dbSet);
        }
    }
}
