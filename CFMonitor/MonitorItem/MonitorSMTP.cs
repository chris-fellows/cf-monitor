using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    [XmlType("MonitorSMTP")]
    public class MonitorSMTP : MonitorItem
    {
        [XmlAttribute("Server")]
        public string Server { get; set; }
        [XmlAttribute("Port")]
        public int Port { get; set; }
    }
}
