using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Services
{
    public class XmlSystemTaskStatusService : XmlEntityWithIdAndNameService<SystemTaskStatus, string>, ISystemTaskStatusService
    {
        public XmlSystemTaskStatusService(string folder) : base(folder,
                                                "SystemTaskStatus.*.xml",
                                              (systemTaskStatus) => $"SystemTaskStatus.{systemTaskStatus.Id}.xml",
                                                (systemTaskStatusId) => $"SystemTaskStatus.{systemTaskStatusId}.xml",
                                                (systemTaskStatus) => systemTaskStatus.Name)
        {

        }
    }
}
