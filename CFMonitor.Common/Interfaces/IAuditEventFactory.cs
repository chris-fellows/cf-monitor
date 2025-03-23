using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Factory for AuditEvent instances
    /// </summary>
    public interface IAuditEventFactory
    {
        AuditEvent CreateActionExecuted(string monitorAgentId, string monitorItemId, string actionItemId);

        AuditEvent CreateCheckedMonitorItem(string monitorAgentId, string monitorItemId);

        AuditEvent CreateCheckingMonitorItem(string monitorAgentId, string monitorItemId);

        AuditEvent CreateUserAdded(string userId);

        AuditEvent CreateMonitorAgentAdded(string monitorAgentId);

        AuditEvent CreateMonitorAgentHeartbeat(string monitorAgentId);

        AuditEvent CreateMonitorItemAdded(string monitorItemId, string userId);

        AuditEvent CreateMonitorItemUpdated(string monitorItemId, string userId);
    }
}
