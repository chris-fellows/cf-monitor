using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.XML;

namespace CFMonitor.Services
{
    /// <summary>
    /// Service for storing MonitorAgent instances in XML format
    /// </summary>
    public class MonitorAgentService : XmlItemRepository<MonitorAgent, string>, IMonitorAgentService
    {
        public MonitorAgentService(string folder) : base(folder, (MonitorAgent monitorAgent) => monitorAgent.ID)
        {

        }
    }
}
