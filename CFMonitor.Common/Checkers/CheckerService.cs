using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks Windows service
    /// </summary>
    public class CheckerService : IChecker
    {
        public string Name => "Service";

        public CheckerTypes CheckerType => CheckerTypes.Service;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
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
                actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking service");
                CheckEvents(actionerList, monitorService, actionParameters, exception, serviceController);
            }
            catch (System.Exception ex)
            {

            }

            return Task.CompletedTask;
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
                if (actioner.CanExecute(actionItem))
                {
                    actioner.ExecuteAsync(monitorItem, actionItem, actionParameters);
                    break;
                }
            }
        }
    }
}
