using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor
{
    /// <summary>
    /// Checks DHCP server
    /// </summary>
    public class CheckerDHCP : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorDHCP monitorDHCP = (MonitorDHCP)monitorItem;
            Exception exception = null;
            ActionParameters actionParameters = new CFMonitor.ActionParameters();

            try
            {

            }
            catch (Exception ex)
            {
                exception = ex;
            }

            try
            {
                CheckEvents(actionerList, monitorDHCP, actionParameters, exception);
            }
            catch (Exception ex)
            {

            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorDHCP;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorDHCP monitorDHCP, ActionParameters actionParameters, Exception exception)
        {
            foreach (EventItem eventItem in monitorDHCP.EventItems)
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

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorDHCP, actionItem, actionParameters);
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
