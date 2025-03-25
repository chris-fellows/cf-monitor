using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    public interface IAuditEventService : IEntityWithIdService<AuditEvent, string>
    {
        List<AuditEvent> GetByFilter(AuditEventFilter filter);
    }
}
