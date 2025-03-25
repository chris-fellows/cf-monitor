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
    public class XmlMonitorItemOutputService : XmlEntityWithIdService<MonitorItemOutput, string>, IMonitorItemOutputService
    {
        public XmlMonitorItemOutputService(string folder) : base(folder,
                                                "MonitorItemOutput.*.xml",
                                              (monitorItemOutput) => $"MonitorItemOutput.{monitorItemOutput.Id}.xml",
                                                (monitorItemOutputId) => $"MonitorItemOutput.{monitorItemOutputId}.xml")
        {

        }
    }
}
