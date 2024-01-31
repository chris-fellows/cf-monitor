using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor
{
    public class CheckerJSON : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorJSON monitorJSON = (MonitorJSON)monitorItem;
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
                CheckEvents(actionerList, monitorJSON, actionParameters, exception);
            }
            catch (Exception ex)
            {

            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorJSON;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorJSON monitorJSON, ActionParameters actionParameters, Exception exception)
        {
            foreach (EventItem eventItem in monitorJSON.EventItems)
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
                        DoAction(actionerList, monitorJSON, actionItem, actionParameters);
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
