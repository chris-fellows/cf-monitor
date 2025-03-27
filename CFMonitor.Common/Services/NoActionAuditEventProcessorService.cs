using CFMonitor.Interfaces;
using CFMonitor.Models;

namespace CFMonitor.Services
{
    /// <summary>
    /// Audit event processor that takes no action. The default processor creates notifications for specific
    /// audit event types (E.g. Reset password, new user etc).
    /// </summary>
    public class NoActionAuditEventProcessorService : IAuditEventProcessorService
    {
        public Task ProcessAsync(AuditEvent auditEvent)
        {
            return Task.CompletedTask;
        }
    }
}
