using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
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
