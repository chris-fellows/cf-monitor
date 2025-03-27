namespace CFMonitor.Interfaces
{
    public interface IEntityWithIdAndNameService<TEntityType, TEntityIdType> : IEntityWithIdService<TEntityType, TEntityIdType>
    {
        TEntityType? GetByName(string name);
    }
}
