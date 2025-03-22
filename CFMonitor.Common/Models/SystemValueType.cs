using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models
{
    /// <summary>
    /// System value type
    /// </summary>
    public class SystemValueType
    {
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// System value type
        /// </summary>
        public SystemValueTypes ValueType { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Data type
        /// </summary>
        public SystemValueDataTypes DataType { get; set; }
                
        /// <summary>
        /// Min value for range. E.g. Integer range.
        /// </summary>
        public string? MinValue { get; set; }

        /// <summary>
        /// Max value for range. E.g. Integer range.
        /// </summary>
        public string? MaxValue { get; set; }
        
        /// <summary>
        /// Default event condition. Only set for system value types that refer to event condition sources (ECS_*)
        /// </summary>
        public EventCondition? DefaultEventCondition { get; set; }
    }
}
