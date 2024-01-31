using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    /// <summary>
    /// Settings for monitoring a URL
    /// </summary>
    [XmlType("MonitorURL")]
    public class MonitorURL : MonitorItem         
    {
        [XmlAttribute("URL")]
        public string URL { get; set; }
        [XmlAttribute("Method")]
        public string Method { get; set; }
        [XmlAttribute("ProxyName")]
        public string ProxyName { get; set; }
        [XmlAttribute("ProxyPort")]
        public int ProxyPort { get; set; }
        [XmlAttribute("Password")]
        public string Password { get; set; }
        [XmlAttribute("UserName")]
        public string UserName { get; set; }
        [XmlArray("Headers")]
        [XmlArrayItem("Header")]
        public List<HTTPHeader> Headers = new List<HTTPHeader>();
    }
}
