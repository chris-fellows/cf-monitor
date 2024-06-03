using System.Xml.Serialization;

namespace CFMonitor.Models.ActionItems
{
    [XmlType("ActionItem")]
    [XmlInclude(typeof(ActionDatadogWarning))]
    [XmlInclude(typeof(ActionEmail))]
    [XmlInclude(typeof(ActionProcess))]
    [XmlInclude(typeof(ActionURL))]
    [XmlInclude(typeof(ActionSMS))]
    [XmlInclude(typeof(ActionSQL))]
    [XmlInclude(typeof(ActionLog))]
    public abstract class ActionItem
    {
        
    }
}
