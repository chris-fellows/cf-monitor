using CFMonitor.Enums;

namespace CFMonitor.Models.MonitorItems
{
    public class MonitorItemParameter
    {
        //public SystemValueTypes SystemValueType { get; set; }
        public string SystemValueTypeId { get; set; } = String.Empty;

        public string Value { get; set; } = String.Empty;
    }
}
