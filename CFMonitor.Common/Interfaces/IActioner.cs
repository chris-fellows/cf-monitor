using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for component then performs an action when a monitor item has been checked and meets the
    /// conditions for the action
    /// </summary>
    public interface IActioner
    {
        void DoAction(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters);
        bool CanAction(ActionItem actionItem);
    }
}
