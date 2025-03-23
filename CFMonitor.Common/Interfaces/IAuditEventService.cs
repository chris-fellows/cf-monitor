using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    public interface IAuditEventService : IEntityWithIdStoreService<AuditEvent, string>
    {
        List<AuditEvent> GetByFilter(AuditEventFilter filter);
    }
}
