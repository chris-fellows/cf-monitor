namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Reads entities
    /// </summary>
    public interface IEntityReader<TEntity>
    {
        Task<List<TEntity>> ReadAllAsync();
    }
}
