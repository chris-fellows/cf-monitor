using CFMonitor.Enums;

namespace CFMonitor.Models.MonitorItems
{
    public class MonitorItemParameter
    {
        public SystemValueTypes SystemValueType { get; set; }

        public string Value { get; set; } = String.Empty;
    }
}
