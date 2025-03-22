using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFWebServer.Services;

namespace CFMonitor.Services
{
    /// <summary>
    /// Service for storing MonitorItem instances in XML format
    /// </summary>
    public class XmlMonitorItemService : XmlEntityWithIdStoreService<MonitorItem, string>, IMonitorItemService
    {
        public XmlMonitorItemService(string folder) : base(folder,
                                                "MonitorItem.*.xml",
                                              (monitorItem) => $"MonitorItem.{monitorItem.Id}.xml",
                                                (monitorItemId) => $"MonitorItem.{monitorItemId}.xml")
        {

        }
    }
}
