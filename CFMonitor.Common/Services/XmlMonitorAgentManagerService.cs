using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Services;
using CFWebServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Services
{
    public class XmlMonitorAgentManagerService : XmlEntityWithIdService<MonitorAgentManager, string>, IMonitorAgentManagerService
    {
        public XmlMonitorAgentManagerService(string folder) : base(folder,
                                                "MonitorAgentManager.*.xml",
                                              (monitorAgentManager) => $"MonitorAgentManager.{monitorAgentManager.Id}.xml",
                                            (monitorAgentManagerId) => $"MonitorAgentManager.{monitorAgentManagerId}.xml")
        {

        }

        public string GetStatusName(MonitorAgentManager monitorAgentManager)
        {
            return monitorAgentManager.HeartbeatDateTime >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(10)) ?
                     "Active" : "Inactive";
        }

        public string GetStatusColor(MonitorAgentManager monitorAgentManager)
        {
            var statusName = GetStatusName(monitorAgentManager);
            return statusName.Equals("Active") ? System.Drawing.Color.Green.ToArgb().ToString() :
                  System.Drawing.Color.Red.ToArgb().ToString();
        }
    }
}
