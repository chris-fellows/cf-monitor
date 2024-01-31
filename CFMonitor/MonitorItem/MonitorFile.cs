using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor 
{   
    /// <summary>
    /// Settings for monitoring a file
    /// </summary>
    [XmlType("MonitorFile")]
    public class MonitorFile : MonitorItem
    {
        [XmlAttribute("FileName")]
        public string FileName { get; set; }
        [XmlAttribute("FindText")]
        public string FindText { get; set; }       // Optional text to find
    }
}
