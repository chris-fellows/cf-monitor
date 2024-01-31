using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    [XmlType("MonitorDNS")]
    public class MonitorDNS : MonitorItem
    {    
        [XmlAttribute("Host")]
        public string Host { get; set; }
    }
}
