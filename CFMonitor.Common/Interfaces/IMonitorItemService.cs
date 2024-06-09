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

    ///// <summary>
    ///// Interface for monitor item service
    ///// </summary>
    //public interface IMonitorItemService
    //{
    //    List<MonitorItem> GetAll();
    //    void Add(MonitorItem monitorItem);
    //    void Update(MonitorItem monitorItem);
    //    void Delete(MonitorItem monitorItem);
    //}
}
