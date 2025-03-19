using CFMonitor.Enums;
using System.Xml.Serialization;

namespace CFMonitor.Models.ActionItems
{
    [XmlType("ActionItem")]
    //[XmlInclude(typeof(ActionDatadogWarning))]
    //[XmlInclude(typeof(ActionEmail))]
    //[XmlInclude(typeof(ActionEventLog))]
    //[XmlInclude(typeof(ActionLog))]
    //[XmlInclude(typeof(ActionMachineRestart))]
    //[XmlInclude(typeof(ActionProcess))]
    //[XmlInclude(typeof(ActionServiceRestart))]
    //[XmlInclude(typeof(ActionURL))]
    //[XmlInclude(typeof(ActionSMS))]
    //[XmlInclude(typeof(ActionSQL))]    
    public abstract class ActionItem
    {
        public ActionerTypes ActionerType { get; set; }

        public List<ActionItemParameter> Parameters = new List<ActionItemParameter>();        
    }
}
