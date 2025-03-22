using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions sending Datadog warning
    /// </summary>
    public class ActionerDatadogWarning : IActioner
    {
        private readonly ISystemValueTypeService _systemValueTypeService;

        public ActionerDatadogWarning(ISystemValueTypeService systemValueTypeService)
        {
            _systemValueTypeService = systemValueTypeService;
        }

        public string Name => "Create Datadog warning";

        //public ActionerTypes ActionerType => ActionerTypes.DatadogWarning;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            //ActionDatadogWarning actionDatadogWarning = (ActionDatadogWarning)actionItem;

            throw new NotImplementedException();

            return Task.CompletedTask;
        }
         
        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.DatadogWarning;
        }
    }
}
