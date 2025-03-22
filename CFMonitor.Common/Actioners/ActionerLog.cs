using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions writing to log file
    /// </summary>
    public class ActionerLog : IActioner
    {
        private readonly ISystemValueTypeService _systemValueTypeService;

        public ActionerLog(ISystemValueTypeService systemValueTypeService)
        {
            _systemValueTypeService = systemValueTypeService;
        }


        public string Name => "Write a log";

        //public ActionerTypes ActionerType => ActionerTypes.Log;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            throw new NotImplementedException();

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.Log;
        }
    }
}
