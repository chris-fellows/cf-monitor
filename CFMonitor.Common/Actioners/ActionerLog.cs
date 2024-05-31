using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions writing to log file
    /// </summary>
    public class ActionerLog : IActioner
    {
        public void DoAction(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {


        }

        public bool CanAction(ActionItem actionItem)
        {
            return actionItem is ActionLog;
        }
    }
}
