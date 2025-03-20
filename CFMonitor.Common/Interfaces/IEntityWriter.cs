namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Writes entities
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityWriter<TEntity>
    {
        Task WriteAllAsync(List<TEntity> entities);
    }
}
