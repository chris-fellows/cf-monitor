﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CFMonitor
{
    /// <summary>
    /// Checks DNS
    /// </summary>
    public class CheckerDNS : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorDNS monitorDNS = (MonitorDNS)monitorItem;
            Exception exception = null;
            IPHostEntry hostEntry = null;
            ActionParameters actionParameters = new CFMonitor.ActionParameters();

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
                if (actioner.CanAction(actionItem))
                {
                    actioner.DoAction(monitorItem, actionItem, actionParameters);
                    break;
                }
            }
        }
    }
}
