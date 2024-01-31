using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    [XmlType("MonitorDHCP")]
    public class MonitorDHCP : MonitorItem
    {
        [XmlAttribute("Server")]
        public string Server { get; set; }
    }
}
