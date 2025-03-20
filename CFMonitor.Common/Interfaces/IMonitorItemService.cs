using CFMonitor.Models.MonitorItems;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for MonitorItem instances
    /// </summary>
    public interface IMonitorItemService : IEntityWithIdStoreService<MonitorItem, string>
    {
    }   
}
