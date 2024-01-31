using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    [XmlType("MonitorSocket")]
    public class MonitorSocket : MonitorItem
    {
        [XmlAttribute("Port")]
        public int Port { get; set; }
        [XmlAttribute("Host")]
        public string Host { get; set; }
        [XmlAttribute("Protocol")]
        public string Protocol { get; set; }            // TCP/UDP
    }
}
