using CFMonitor.Interfaces;
using CFMonitor.Models.MonitorItems;
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
                                              (monitorItem) => $"MonitorAgent.{monitorItem.ID}.xml",
                                                (monitorItemId) => $"MonitorAgent.{monitorItemId}.xml")
        {

        }
    }
}
