using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions restart of machine
    /// </summary>
    public class ActionerMachineRestart : IActioner
    {
        private readonly ISystemValueTypeService _systemValueTypeService;

        public ActionerMachineRestart(ISystemValueTypeService systemValueTypeService)
        {
            _systemValueTypeService = systemValueTypeService;
        }

        public string Name => "Restart machine";

        //public ActionerTypes ActionerType => ActionerTypes.MachineRestart;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
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
