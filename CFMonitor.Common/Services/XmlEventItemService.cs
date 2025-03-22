using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFWebServer.Services;

namespace CFMonitor.Services
{
    /// <summary>
    /// Service for storing EventItem instances in XML format
    /// </summary>
    public class XmlEventItemService : XmlEntityWithIdStoreService<EventItem, string>, IEventItemService
    {
        public XmlEventItemService(string folder) : base(folder,
                                                "EventItem.*.xml",
                                              (eventItem) => $"EventItem.{eventItem.Id}.xml",
                                                (eventItemId) => $"EventItem.{eventItemId}.xml")
        {

        }

        public List<EventItem> GetByMonitorItemId(string monitorItemId)
        {
            return GetAll().Where(ei => ei.MonitorItemId == monitorItemId).ToList();
        }
    }
}
