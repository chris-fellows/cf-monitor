using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace CFMonitor
{
    /// <summary>
    /// Checks Windows service
    /// </summary>
    public class CheckerService : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorService monitorService = (MonitorService)monitorItem;
            ServiceController serviceController = null;
            Exception exception = null;
            ActionParameters actionParameters = new ActionParameters();

            try
            {
                // Check if service exists
                ServiceController[] serviceControllers = String.IsNullOrEmpty(monitorService.MachineName) ? 
                            ServiceController.GetServices() : ServiceController.GetServices(monitorService.MachineName);
                foreach(ServiceController currentServiceController in serviceControllers)
                {                    
                    if (currentServiceController.ServiceName.Equals(monitorService.ServiceName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        serviceController = currentServiceController;
                        break;
                    }
                }
              
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            try
            {
                // Check events
                actionParameters.Values.Add("Body", "Error checking service");
                CheckEvents(actionerList, monitorService, actionParameters, exception, serviceController);
            }
            catch (System.Exception ex)
            {

            }
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorService monitorService, ActionParameters actionParameters, Exception exception, ServiceController serviceController)
        {
            foreach (EventItem eventItem in monitorService.EventItems)
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
                    case EventConditionSource.ServiceControllerStatus:
                        meetsCondition = (serviceController != null && eventItem.EventCondition.Evaluate(serviceController.Status));
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
                else if (eventItem.EventCondition.Source.Equals("ServiceController.Status"))
                {
                    meetsCondition = (serviceController != null && eventItem.EventCondition.Evaluate(serviceController.Status));
                } 
                */

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorService, actionItem, actionParameters);
                    }
                }
            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorService;
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
