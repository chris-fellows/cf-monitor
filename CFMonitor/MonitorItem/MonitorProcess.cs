using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    /// <summary>
    /// Settings for monitoring a process
    /// </summary>
    [XmlType("MonitorProcess")]
    public class MonitorProcess : MonitorItem
    {
        [XmlAttribute("MachineName")]
        public string MachineName { get; set; }
        [XmlAttribute("FileName")]
        public string FileName { get; set; }
    }
}
