﻿using CFMonitor.Enums;

namespace CFMonitor.Models
{
    /// <summary>
    /// Monitor item type
    /// </summary>
    public class MonitorItemType
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Name for display
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Monitor item type
        /// </summary>
        public MonitorItemTypes ItemType { get; set; }

        /// <summary>
        /// Event conditions that are relevant
        /// </summary>        
        public List<SystemValueTypes> EventConditionValueTypes { get; set; } = new List<SystemValueTypes>();
        
        /// <summary>
        /// Default parameters
        /// </summary>

        public List<MonitorItemParameter> DefaultParameters = new List<MonitorItemParameter>();

        public string Color { get; set; } = String.Empty;

        public string ImageSource { get; set; } = String.Empty;
    }
}
