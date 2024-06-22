using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions restart of service
    /// </summary>
    public class ActionerServiceRestart : IActioner
    {
        public string Name => "Restart service";

        public ActionerTypes ActionerType => ActionerTypes.ServiceRestart;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            ActionServiceRestart actionServiceRestart = (ActionServiceRestart)actionItem;

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem is ActionLog;
        }
    }
}
