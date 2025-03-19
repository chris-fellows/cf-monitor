using CFMonitor.Enums;

namespace CFMonitor.Models.ActionItems
{
    public class ActionItemParameter
    {
        public SystemValueTypes SystemValueType { get; set; }

        public string Value { get; set; } = String.Empty;
    }
}
