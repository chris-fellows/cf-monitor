using System.Xml.Serialization;

namespace CFMonitor.Models.ActionItems
{
    /// <summary>
    /// Details for action to create Datadog warning
    /// </summary>
    [XmlType("ActionDatadogWarning")]
    public class ActionDatadogWarning : ActionItem
    {
    }
}
