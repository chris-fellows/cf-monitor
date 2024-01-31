using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    [XmlType("ActionLog")]
    public class ActionLog : ActionItem
    {
        [XmlAttribute("LogFileName")]
        public string LogFileName;
    }
}
