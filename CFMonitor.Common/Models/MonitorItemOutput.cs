using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models
{
    public class MonitorItemOutput
    {  
        /// <summary>
        /// Event items that need action
        /// </summary>
        public List<string> EventItemIdsForAction { get; set; } = new();

        /// <summary>
        /// Parameters for actions
        /// </summary>
        public List<ActionItemParameter> ActionItemParameters { get; set; } = new();
    }
}
