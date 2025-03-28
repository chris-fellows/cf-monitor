using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for MonitorAgent instances
    /// </summary>
    public interface IMonitorAgentService : IEntityWithIdService<MonitorAgent, string>
    {
        string GetStatusName(MonitorAgent monitorAgent);

        string GetStatusColor(MonitorAgent monitorAgent);
    }
}
