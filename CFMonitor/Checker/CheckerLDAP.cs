using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor
{
    /// <summary>
    /// Checks LDAP
    /// </summary>
    public class CheckerLDAP : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorLDAP monitorLDAP = (MonitorLDAP)monitorItem;
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
                CheckEvents(actionerList, monitorLDAP, actionParameters, exception);
            }
            catch (Exception ex)
            {

            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorLDAP;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorLDAP monitorLDAP, ActionParameters actionParameters, Exception exception)
        {
            foreach (EventItem eventItem in monitorLDAP.EventItems)
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
                        DoAction(actionerList, monitorLDAP, actionItem, actionParameters);
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
