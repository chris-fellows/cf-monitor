using CFMonitor.Enums;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models
{
    /// <summary>
    /// Monitor item type
    /// </summary>
    public class MonitorItemType
    {
        /// <summary>
        /// Name for display
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        public MonitorItemTypes ItemType { get; set; }
       
        /// <summary>
        /// Function to create instance of monitor item with default properties
        /// </summary>
        public Func<MonitorItem> CreateMonitorItem;

        /// <summary>
        /// Checker type
        /// </summary>
        public CheckerTypes CheckerType { get; set; }
    }
}
