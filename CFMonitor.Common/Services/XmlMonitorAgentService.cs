using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Utilities;
using CFWebServer.Services;

namespace CFMonitor.Services
{
    /// <summary>
    /// Service for storing MonitorAgent instances in XML format
    /// </summary>
    public class XmlMonitorAgentService : XmlEntityWithIdService<MonitorAgent, string>, IMonitorAgentService
    {
        public XmlMonitorAgentService(string folder) : base(folder,
                                                "MonitorAgent.*.xml",
                                              (monitorAgent) => $"MonitorAgent.{monitorAgent.Id}.xml",
                                            (monitorAgentId) => $"MonitorAgent.{monitorAgentId}.xml")
        {

        }

        public string GetStatusName(MonitorAgent monitorAgent)
        {
            return monitorAgent.HeartbeatDateTime >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(10)) ?
                     "Active" : "Inactive";
        }

        public string GetStatusColor(MonitorAgent monitorAgent)
        {
            var statusName = GetStatusName(monitorAgent);
            return statusName.Equals("Active") ? System.Drawing.Color.Green.ToArgb().ToString() :
                  System.Drawing.Color.Red.ToArgb().ToString();
        }
    }
}
