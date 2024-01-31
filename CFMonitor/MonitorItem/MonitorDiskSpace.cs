using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    [XmlType("MonitorDiskSpace")]
    public class MonitorDiskSpace : MonitorItem
    {
        [XmlAttribute("Drive")]
        public string Drive { get; set; }
    }
}
