using CFMonitor.Models.MonitorItems;
using CFUtilities.Repository;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for MonitorItem instances
    /// </summary>
    public interface IMonitorItemService : IItemRepository<MonitorItem, string>
    {
    }   
}
