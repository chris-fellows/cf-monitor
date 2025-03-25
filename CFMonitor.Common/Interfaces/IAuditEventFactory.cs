using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Factory for AuditEvent instances
    /// </summary>
    public interface IAuditEventFactory
    {
        AuditEvent CreateActionExecuted(string createdUserId, string monitorItemOutputId, string actionItemId);

        AuditEvent CreateCheckedMonitorItem(string createdUserId, string monitorItemOutputId);

        AuditEvent CreateCheckingMonitorItem(string createdUserId, string monitorAgentId, string monitorItemId);

        AuditEvent CreateError(string createdUserId, string errorMessage, List<AuditEventParameter> parameters);

        AuditEvent CreateUserAdded(string createdUserId, string userId);

        AuditEvent CreateMonitorAgentAdded(string createdUserId, string monitorAgentId);

        AuditEvent CreateMonitorAgentHeartbeat(string createdUserId, string monitorAgentId);

        AuditEvent CreateMonitorItemAdded(string createdUserId, string monitorItemId, string userId);

        AuditEvent CreateMonitorItemUpdated(string createdUserId, string monitorItemId, string userId);

        AuditEvent CreatePasswordResetAdded(string createdUserId, string passwordResetId);

        AuditEvent CreatePasswordUpdated(string createdUserId, string userId);

        AuditEvent CreateUserLogInSuccess(string createdUserId, string userId);

        AuditEvent CreateUserLogOut(string createdUserId, string userId);

        AuditEvent CreateUserLogInError(string createdUserId, string username);
    }
}
