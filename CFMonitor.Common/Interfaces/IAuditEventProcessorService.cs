using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Processes audit event. E.g. Creates a notification.
    /// </summary>
    public interface IAuditEventProcessorService
    {
        Task ProcessAsync(AuditEvent auditEvent);
    }
}
