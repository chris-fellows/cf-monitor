using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    public interface IEventItemFactoryService
    {
        /// <summary>
        /// Returns default list of event items for monitor item. Actions list will be empty and so no action
        /// will be taken if the event condition is valid.
        /// </summary>
        /// <param name="monitorItem"></param>
        /// <returns></returns>
        List<EventItem> GetDefaultForMonitorItem(MonitorItem monitorItem);
    }
}
