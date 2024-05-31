using System.Xml.Serialization;

namespace CFMonitor.Models.ActionItems
{
    [XmlType("ActionLog")]
    public class ActionLog : ActionItem
    {
        [XmlAttribute("LogFileName")]
        public string LogFileName;
    }
}
