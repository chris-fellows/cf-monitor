using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    public interface IAuditEventTypeService : IEntityWithIdAndNameStoreService<AuditEventType, string>
    {
    }
}
