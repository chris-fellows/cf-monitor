using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
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
