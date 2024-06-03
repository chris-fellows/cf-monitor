using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions sending SMS text
    /// </summary>
    public class ActionerSMS : IActioner
    {
        public string Name => "Send SMS text";

        public ActionerTypes ActionerType => ActionerTypes.SMS;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            ActionSMS actionSMS = (ActionSMS)actionItem;

            throw new NotImplementedException();

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem is ActionSMS;
        }
    }
}
