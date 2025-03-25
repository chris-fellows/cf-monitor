namespace CFMonitor.Interfaces
{
    public interface IEntityWithIdAndNameStoreService<TEntityType, TEntityIdType> : IEntityWithIdService<TEntityType, TEntityIdType>
    {
        TEntityType? GetByName(string name);
    }
}
