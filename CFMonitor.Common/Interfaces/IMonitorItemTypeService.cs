using CFMonitor.Models;
using System.Collections.Generic;

namespace CFMonitor.Interfaces
{
    public interface IMonitorItemTypeService
    {
        List<MonitorItemType> GetAll();
    }
}
