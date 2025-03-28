namespace CFMonitor.Interfaces
{
    public interface IEntityWithIdAndNameService<TEntityType, TEntityIdType> : IEntityWithIdService<TEntityType, TEntityIdType>
    {
        /// <summary>
        /// Gets entity by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<TEntityType?> GetByNameAsync(string name);

        //TEntityType? GetByName(string name);
    }
}
