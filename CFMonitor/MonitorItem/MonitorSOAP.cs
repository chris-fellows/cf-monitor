using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    [XmlType("MonitorSOAP")]
    public class MonitorSOAP : MonitorItem
    {
        [XmlAttribute("URL")]
        public string URL { get; set; }
        [XmlElement("XML")]
        public string XML { get; set; }
    }
}
