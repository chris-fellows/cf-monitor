using CFMonitor.Models.MonitorItems;
using System.Collections.Generic;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for component to store the status of the monitor items.
    /// 
    /// Could be a CSV file or a web page with a table listing each item
    /// </summary>
    public interface IMonitorItemsStatus
    {
        void Add(List<MonitorItem> monitorItems);
    }
}
