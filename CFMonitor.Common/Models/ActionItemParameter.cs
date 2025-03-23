using CFMonitor.Enums;

namespace CFMonitor.Models
{
    /// <summary>
    /// Parameter for IActioner
    /// </summary>
    public class ActionItemParameter
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
