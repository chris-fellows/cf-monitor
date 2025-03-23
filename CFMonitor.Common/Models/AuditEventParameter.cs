namespace CFMonitor.Models
{
    public class AuditEventParameter
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
