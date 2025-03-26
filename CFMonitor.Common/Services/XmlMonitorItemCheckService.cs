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
    public class XmlMonitorItemCheckService : XmlEntityWithIdService<MonitorItemCheck, string>, IMonitorItemCheckService
    {
        public XmlMonitorItemCheckService(string folder) : base(folder,
                                                "MonitorItemCheck.*.xml",
                                              (monitorItemCheck) => $"MonitorItemCheck.{monitorItemCheck.Id}.xml",
                                                (monitorItemCheckId) => $"MonitorItemCheck.{monitorItemCheckId}.xml")
        {

        }
    }
}
