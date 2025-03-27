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
    public class XmlSystemTaskTypeService : XmlEntityWithIdAndNameService<SystemTaskType, string>, ISystemTaskTypeService
    {
        public XmlSystemTaskTypeService(string folder) : base(folder,
                                                "SystemTaskType.*.xml",
                                              (systemTaskType) => $"SystemTaskType.{systemTaskType.Id}.xml",
                                                (systemTaskTypeId) => $"SystemTaskType.{systemTaskTypeId}.xml",
                                                (systemTaskType) => systemTaskType.Name)
        {

        }
    }
}
