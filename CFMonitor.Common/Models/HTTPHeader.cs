using System.Xml.Serialization;

namespace CFMonitor.Models
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
