using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    /// <summary>
    /// Settings for monitoring a server by ping
    /// </summary>
    [XmlType("MonitorPing")]
    public class MonitorPing : MonitorItem
    {
        [XmlAttribute("Server")]
        public string Server { get; set; }
    }
}
