using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Factory for AuditEvent instances
    /// </summary>
    public interface IAuditEventFactory
    {
        AuditEvent CreateActionExecuted(string monitorItemOutputId, string actionItemId);

        AuditEvent CreateCheckedMonitorItem(string monitorItemOutputId);

        AuditEvent CreateCheckingMonitorItem(string monitorAgentId, string monitorItemId);

        AuditEvent CreateError(string errorMessage, List<AuditEventParameter> parameters);

        AuditEvent CreateUserAdded(string userId);

        AuditEvent CreateMonitorAgentAdded(string monitorAgentId);

        AuditEvent CreateMonitorAgentHeartbeat(string monitorAgentId);

        AuditEvent CreateMonitorItemAdded(string monitorItemId, string userId);

        AuditEvent CreateMonitorItemUpdated(string monitorItemId, string userId);
    }
}
