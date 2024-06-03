using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions writing to log file
    /// </summary>
    public class ActionerLog : IActioner
    {
        public string Name => "Write a log";

        public ActionerTypes ActionerType => ActionerTypes.Log;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            throw new NotImplementedException();

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem is ActionLog;
        }
    }
}
