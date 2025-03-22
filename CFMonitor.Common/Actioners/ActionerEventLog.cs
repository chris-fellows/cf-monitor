using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions writing to Event Log
    /// </summary>
    public class ActionerEventLog : IActioner
    {
        private readonly ISystemValueTypeService _systemValueTypeService;

        public ActionerEventLog(ISystemValueTypeService systemValueTypeService)
        {
            _systemValueTypeService = systemValueTypeService;
        }


        public string Name => "Write to Event Log";

        //public ActionerTypes ActionerType => ActionerTypes.Log;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            throw new NotImplementedException();

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.EventLog;                 
        }
    }
}
