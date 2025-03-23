using CFMonitor.Enums;

namespace CFMonitor.Models
{
    /// <summary>
    /// Parameter for IChcker
    /// </summary>
    public class MonitorItemParameter
    {        
        public string SystemValueTypeId { get; set; } = String.Empty;

        public string Value { get; set; } = String.Empty;

        public SystemValue ToSystemValue()
        {
            return new SystemValue()
            {
                TypeId = SystemValueTypeId,
                Value = Value
            };
        }
    }
}
