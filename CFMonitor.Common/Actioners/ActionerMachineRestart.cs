using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions restart of machine
    /// </summary>
    public class ActionerMachineRestart : IActioner
    {
        public string Name => "Restart machine";

        public ActionerTypes ActionerType => ActionerTypes.MachineRestart;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            //ActionMachineRestart actionMachineRestart = (ActionMachineRestart)actionItem;

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.MachineRestart;
        }
    }
}
