using System.Xml.Serialization;

namespace CFMonitor.Models.ActionItems
{
    /// <summary>
    /// Details for action to run process
    /// </summary>
    [XmlType("ActionProcess")]
    public class ActionProcess : ActionItem
    {
        [XmlAttribute("FileName")]
        public string FileName { get; set; }
    }
}
