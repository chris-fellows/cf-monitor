using CFMonitor.Enums;

namespace CFMonitor.Models
{
    public class AuditEventType
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public AuditEventTypes EventType { get; set; }

        public string Color { get; set; } = String.Empty;

        public string ImageSource { get; set; } = String.Empty;

        public List<string> NotificationGroupIds { get; set; } = new();
    }
}
