using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks LDAP
    /// </summary>
    public class CheckerLDAP : CheckerBase, IChecker
    {        

        public CheckerLDAP(IEventItemService eventItemService, 
                        ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService)
        {
            
        }

        public string Name => "LDAP";

        //public CheckerTypes CheckerType => CheckerTypes.LDAP;

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

                //MonitorLDAP monitorLDAP = (MonitorLDAP)monitorItem;
                Exception exception = null;
                ActionParameters actionParameters = new ActionParameters();

                try
                {

                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                try
                {
                    foreach (var eventItem in eventItems)
                    {
                        await CheckEventAsync(eventItem, actionerList, monitorItem, actionParameters, exception);
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.LDAP;
        }

        private async Task CheckEventAsync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorLDAP, ActionParameters actionParameters, Exception exception)
        {            
                bool meetsCondition = false;
                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        await ExecuteActionAsync(actionerList, monitorLDAP, actionItem, actionParameters);
                    }
                }            
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
