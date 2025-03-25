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
    public class XmlActionItemTypeService : XmlEntityWithIdAndNameService<ActionItemType, string>, IActionItemTypeService
    {
        public XmlActionItemTypeService(string folder) : base(folder,
                                                "ActionItemType.*.xml",
                                                (actionItemType) => $"ActionItemType.{actionItemType.Id}.xml",
                                                (actionItemTypeId) => $"ActionItemType.{actionItemTypeId}.xml",
                                                (actionItemType) => actionItemType.Name)
        {

        }
    }
}
