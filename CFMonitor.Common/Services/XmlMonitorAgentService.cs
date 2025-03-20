using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFWebServer.Services;

namespace CFMonitor.Services
{
    /// <summary>
    /// Service for storing MonitorAgent instances in XML format
    /// </summary>
    public class XmlMonitorAgentService : XmlEntityWithIdStoreService<MonitorAgent, string>, IMonitorAgentService
    {
        public XmlMonitorAgentService(string folder) : base(folder,
                                                "MonitorAgent.*.xml",
                                              (monitorAgent) => $"MonitorAgent.{monitorAgent.ID}.xml",
                                            (monitorAgentId) => $"MonitorAgent.{monitorAgentId}.xml")
        {

        }
    }
}
