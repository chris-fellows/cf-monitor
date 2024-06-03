using CFMonitor.Enums;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System.Threading.Tasks;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for component then performs an action when a monitor item has been checked and meets the
    /// conditions for the action
    /// </summary>
    public interface IActioner
    { 
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }
        
        ActionerTypes ActionerType { get; }

        /// <summary>
        /// Executes action
        /// </summary>
        /// <param name="monitorItem"></param>
        /// <param name="actionItem"></param>
        /// <param name="actionParameters"></param>
        Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters);

        /// <summary>
        /// Determines if this nstance can execute action
        /// </summary>
        /// <param name="actionItem"></param>
        /// <returns></returns>
        bool CanExecute(ActionItem actionItem);
    }
}
