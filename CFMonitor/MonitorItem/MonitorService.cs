using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    /// <summary>
    /// Settings for monitoring a service
    /// </summary>
    [XmlType("MonitorService")]
    public class MonitorService : MonitorItem
    {
        [XmlAttribute("MachineName")]
        public string MachineName { get; set; }
        [XmlAttribute("ServiceName")]
        public string ServiceName { get; set; }
    }
}
