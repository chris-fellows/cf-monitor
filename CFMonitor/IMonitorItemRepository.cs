using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor
{
    /// <summary>
    /// Interface to repository for monitor items
    /// </summary>
    interface IMonitorItemRepository
    {
        List<MonitorItem> GetAll();
        void Insert(MonitorItem monitorItem);
        void Update(MonitorItem monitorItem);
        void Delete(MonitorItem monitorItem);
    }
}
