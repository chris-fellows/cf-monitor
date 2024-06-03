using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks DNS
    /// </summary>
    public class CheckerDNS : IChecker
    {
        public string Name => "DNS";

        public CheckerTypes CheckerType => CheckerTypes.DNS;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorDNS monitorDNS = (MonitorDNS)monitorItem;
            Exception exception = null;
            IPHostEntry hostEntry = null;
            ActionParameters actionParameters = new ActionParameters();

            try
            {
                hostEntry = Dns.GetHostEntry(monitorDNS.Host);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            try
            {
                CheckEvents(actionerList, monitorDNS, actionParameters, exception, hostEntry);
            }
            catch (Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorDNS;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorDNS monitorDNS, ActionParameters actionParameters, Exception exception, IPHostEntry hostEntry)
        {
            foreach (EventItem eventItem in monitorDNS.EventItems)
            {                            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.Source)
                {
                    case EventConditionSource.Exception:
                        meetsCondition = (exception != null);
                        break;
                    case EventConditionSource.NoException:
                        meetsCondition = (exception == null);
                        break;
                    case EventConditionSource.HostEntryExists:
                        meetsCondition = (hostEntry != null);
                        break;
                    case EventConditionSource.HostEntryNotExists:
                        meetsCondition = (hostEntry == null);
                        break;
                }        
                 
                /*
                if (eventItem.EventCondition.Source.Equals("OnException"))
                {
                    meetsCondition = (exception != null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnNoException"))
                {
                    meetsCondition = (exception == null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnHostEntryExists"))
                {
                    meetsCondition = (hostEntry != null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnHostEntryNotExists"))
                {
                    meetsCondition = (hostEntry == null);
                }
                */

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorDNS, actionItem, actionParameters);
                    }
                }
            }
        }

        private void DoAction(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            foreach (IActioner actioner in actionerList)
            {
                if (actioner.CanExecute(actionItem))
                {
                    actioner.ExecuteAsync(monitorItem, actionItem, actionParameters);
                    break;
                }
            }
        }
    }
}
