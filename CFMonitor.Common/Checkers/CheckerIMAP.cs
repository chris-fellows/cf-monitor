using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks result of IMAP server connection
    /// </summary>
    public class CheckerIMAP : CheckerBase, IChecker
    {        
        public string Name => "IMAP";

        public CheckerIMAP(IEventItemService eventItemService,
            ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService) 
        {
            
        }

        //public CheckerTypes CheckerType => CheckerTypes.IMAP;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            return Task.Factory.StartNew(async () =>
            {
                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return;
                }

                //MonitorIMAP monitorIMAP = (MonitorIMAP)monitorItem;
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
                    actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking IMAP server");
                    foreach (var eventItem in eventItems)
                    {
                        await CheckEventAsync(eventItem, actionerList, monitorItem, actionParameters, exception);
                    }
                }
                catch (System.Exception ex)
                {

                }
            });
        }

        private async Task CheckEventAsync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorIMAP, ActionParameters actionParameters, Exception exception)
        {            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_IMAPConnected:
                        meetsCondition = eventItem.EventCondition.IsValid(true);    // TODO: Set this
                        break;
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        await ExecuteActionAsync(actionerList, monitorIMAP, actionItem, actionParameters);
                    }
                }            
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.IMAP;
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
