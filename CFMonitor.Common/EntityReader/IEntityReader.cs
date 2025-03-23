namespace CFMonitor.EntityReader
{
    /// <summary>
    /// Reads entities
    /// </summary>
    public interface IEntityReader<TEntity>
    {
        IEnumerable<TEntity> Read();
    }
}
