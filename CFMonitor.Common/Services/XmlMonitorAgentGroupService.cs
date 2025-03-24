using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFWebServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Services
{
    public class XmlMonitorAgentGroupService : XmlEntityWithIdStoreService<MonitorAgentGroup, string>, IMonitorAgentGroupService
    {
        public XmlMonitorAgentGroupService(string folder) : base(folder,
                                                "MonitorAgentGroup.*.xml",
                                              (monitorAgentGroup) => $"MonitorAgentGroup.{monitorAgentGroup.Id}.xml",
                                                (monitorAgentGroupId) => $"MonitorAgentGroup.{monitorAgentGroupId}.xml")
        {

        }
    }
}
