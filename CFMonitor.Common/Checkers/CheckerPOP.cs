﻿using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks result of POP server connection
    /// </summary>
    public class CheckerPOP : IChecker
    {
        public string Name => "POP";

        public CheckerTypes CheckerType => CheckerTypes.POP;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            MonitorPOP monitorPOP = (MonitorPOP)monitorItem;
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
                actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking POP server");
                CheckEvents(actionerList, monitorPOP, actionParameters, exception);
            }
            catch (System.Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorPOP monitorPOP, ActionParameters actionParameters, Exception exception)
        {
            foreach (EventItem eventItem in monitorPOP.EventItems)
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
                    case EventConditionSource.POPConnected:
                        // TODO: Set this
                        break;
                    case EventConditionSource.POPConnectError:
                        break;
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorPOP, actionItem, actionParameters);
                    }
                }
            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorNTP;
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
