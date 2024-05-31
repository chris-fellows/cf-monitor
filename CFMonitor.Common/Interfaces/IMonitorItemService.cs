using CFMonitor.Models.MonitorItems;
using System.Collections.Generic;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for monitor item service
    /// </summary>
    public interface IMonitorItemService
    {
        List<MonitorItem> GetAll();
        void Add(MonitorItem monitorItem);
        void Update(MonitorItem monitorItem);
        void Delete(MonitorItem monitorItem);
    }
}
