using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    /// <summary>
    /// Settings for monitoring a SQL database by running a query
    /// </summary>
    [XmlType("MonitorSQL")]
    public class MonitorSQL : MonitorItem
    {
        [XmlAttribute("ConnectionString")]
        public string ConnectionString { get; set; }
        [XmlAttribute("QueryFile")]
        public string QueryFile { get; set; }
    }
}
