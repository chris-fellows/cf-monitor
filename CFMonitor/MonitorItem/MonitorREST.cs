using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    [XmlType("MonitorREST")]
    public class MonitorREST : MonitorItem
    {
        [XmlAttribute("URL")]
        public string URL { get; set; }
    }
}
