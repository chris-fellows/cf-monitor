using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
