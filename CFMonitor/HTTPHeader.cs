using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    [XmlType("HTTPHeader")]
    public class HTTPHeader
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("Value")]
        public string Value { get; set; }
    }
}
