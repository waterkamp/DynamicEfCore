namespace DynamicEfCore
{
    public interface IDynamicDbSet
    {
        object DbSet { get; }

        Type EntityType { get; }

        object ToList();

        object SearchByTerm(string searchTerm, string targetProperty);
    }
}
