namespace CFMonitor.EntityWriter
{
    /// <summary>
    /// Writes entities
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityWriter<TEntity>
    {
        void Write(IEnumerable<TEntity> entities);
    }
}
