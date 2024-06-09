using CFMonitor.Models;
using CFUtilities.Repository;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for MonitorAgent instances
    /// </summary>
    public interface IMonitorAgentService : IItemRepository<MonitorAgent, string>
    {
    }
}
