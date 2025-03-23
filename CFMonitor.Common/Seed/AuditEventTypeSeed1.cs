using CFMonitor.EntityReader;
using CFMonitor.Enums;
using CFMonitor.Models;
using System.Drawing;

namespace CFMonitor.Seed
{
    public class AuditEventTypeSeed1 : IEntityReader<AuditEventType>
    {
        public IEnumerable<AuditEventType> Read()
        {
            var items = new List<AuditEventType>()
            {
                 new AuditEventType()
               {
                   Id = Guid.NewGuid().ToString(),
                   EventType = AuditEventTypes.ActionExecuted,
                   Name = "Action executed",
                   Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "audit_event_type.png",
               },
               new AuditEventType()
               {
                   Id =Guid.NewGuid().ToString(),
                   EventType = AuditEventTypes.CheckedMonitorItem,
                   Name = "Checked monitor item",
                   Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "audit_event_type.png",
               },
               new AuditEventType()
               {
                   Id =Guid.NewGuid().ToString(),
                   EventType = AuditEventTypes.CheckingMonitorItem,
                   Name = "Checking monitor item",
                   Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "audit_event_type.png",
               },
               new AuditEventType()
               {
                   Id =Guid.NewGuid().ToString(),
                   EventType = AuditEventTypes.MonitorAgentAdded,
                   Name = "Monitor agent added",
                   Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "audit_event_type.png",
               },
               new AuditEventType()
               {
                   Id =Guid.NewGuid().ToString(),
                   EventType = AuditEventTypes.MonitorAgentHeartbeat,
                   Name = "Monitor agent heartbeat",
                   Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "audit_event_type.png",
               },
               new AuditEventType()
               {
                   Id =Guid.NewGuid().ToString(),
                   EventType = AuditEventTypes.MonitorItemAdded,
                   Name = "Monitor item added",
                   Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "audit_event_type.png",
               },
               new AuditEventType()
               {
                   Id =Guid.NewGuid().ToString(),
                   EventType = AuditEventTypes.MonitorItemDeleted,
                   Name = "Monitor item deleted",
                   Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "audit_event_type.png",
               },
               new AuditEventType()
               {
                   Id =Guid.NewGuid().ToString(),
                   EventType = AuditEventTypes.MonitorItemUpdated,
                   Name = "Monitor item updated",
                   Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "audit_event_type.png",
               },
               new AuditEventType()
               {
                   Id =Guid.NewGuid().ToString(),
                   EventType = AuditEventTypes.UserAdded,
                   Name = "User added",
                   Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "audit_event_type.png",
               },
               new AuditEventType()
               {
                   Id =Guid.NewGuid().ToString(),
                   EventType = AuditEventTypes.UserDeleted,
                   Name = "User deleted",
                   Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "audit_event_type.png",
               },
               new AuditEventType()
               {
                   Id =Guid.NewGuid().ToString(),
                   EventType = AuditEventTypes.UserUpdated,
                   Name = "User updated",
                   Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "audit_event_type.png",
               },
            };

            return items;
        }
    }
}
