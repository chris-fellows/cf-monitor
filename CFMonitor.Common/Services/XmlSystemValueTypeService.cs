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
    public class XmlSystemValueTypeService : XmlEntityWithIdStoreService<SystemValueType, string>, ISystemValueTypeService
    {
        public XmlSystemValueTypeService(string folder) : base(folder,
                                                "SystemValueType.*.xml",
                                              (systemValueType) => $"SystemValueType.{systemValueType.Id}.xml",
                                                (systemValueTypeId) => $"SystemValueType.{systemValueTypeId}.xml")
        {

        }
    }
}
