using CFMonitor.Models.ActionItems;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models
{
    /// <summary>
    /// Details for a particular event; The condition for determining if the event has happened and the actions to
    /// take if it has happened.
    /// </summary>
    [XmlType("EventItem")]
    public class EventItem
    {
        [XmlElement("EventCondition")]
        public EventCondition EventCondition = new EventCondition();  
        
        [XmlArray("ActionItems")]
        [XmlArrayItem("ActionItem")]
        public List<ActionItem> ActionItems = new List<ActionItem>();
    }
}
