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

        public CheckerIMAP(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
            IEventItemService eventItemService,
            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, systemValueTypeService) 
        {
            
        }

        //public CheckerTypes CheckerType => CheckerTypes.IMAP;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem,  bool testMode)
        {
            return Task.Factory.StartNew(() =>
            {
                var monitorItemOutput = new MonitorItemOutput();

                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return monitorItemOutput;
                }

                //MonitorIMAP monitorIMAP = (MonitorIMAP)monitorItem;
                Exception exception = null;
                var actionItemParameters = new List<ActionItemParameter>();

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
                    //actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking IMAP server");
                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem, monitorItem, actionItemParameters, exception))
                        {
                            monitorItemOutput.EventItemIdsForAction.Add(eventItem.Id);
                        }
                    }
                }
                catch (System.Exception ex)
                {

                }

                return monitorItemOutput;
            });
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorIMAP, List<ActionItemParameter> actionItemParameters, Exception exception)
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

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorIMAP, actionItem, actionParameters);
            //    }
            //}
            
            return meetsCondition;
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
