using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks result of POP server connection
    /// </summary>
    public class CheckerPOP : CheckerBase, IChecker
    {
        

        public CheckerPOP(IEventItemService eventItemService,
            ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService)
        {
            
        }

        public string Name => "POP";

        //public CheckerTypes CheckerType => CheckerTypes.POP;

        public  Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            return Task.Factory.StartNew(async () =>
            {
                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return;
                }

                //MonitorPOP monitorPOP = (MonitorPOP)monitorItem;
                Exception exception = null;
                ActionParameters actionParameters = new ActionParameters();

                try
                {

                }
                catch (System.Exception ex)
                {
                    exception = ex;
                }

                try
                {
                    // Check events
                    actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking POP server");
                    foreach (var eventItem in eventItems)
                    {
                        CheckEventAsync(eventItem, actionerList, monitorItem, actionParameters, exception);
                    }
                }
                catch (System.Exception ex)
                {

                }

                return;
            });
        }

        private async Task CheckEventAsync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorPOP, ActionParameters actionParameters, Exception exception)
        {            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_POPConnected:
                        meetsCondition = eventItem.EventCondition.IsValid(true);    // TODO: Set this
                        break;
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        await ExecuteActionAsync(actionerList, monitorPOP, actionItem, actionParameters);
                    }
                }            
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.POP;                 
        }

        //private void DoAction(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        //{
        //    foreach (IActioner actioner in actionerList)
        //    {
        //        if (actioner.CanExecute(actionItem))
        //        {
        //            actioner.ExecuteAsync(monitorItem, actionItem, actionParameters);
        //            break;
        //        }
        //    }
        //}
    }
}
