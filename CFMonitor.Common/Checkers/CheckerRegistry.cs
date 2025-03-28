﻿//using CFMonitor.Enums;
//using CFMonitor.Interfaces;
//using CFMonitor.Models;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace CFMonitor.Checkers
//{
//    /// <summary>
//    /// Checks registry
//    /// </summary>
//    public class CheckerRegistry : IChecker
//    {
//        public string Name => "Registry";

//        public CheckerTypes CheckerType => CheckerTypes.Registry;

//        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
//        {
//            //MonitorRegistry monitorRegistry = (MonitorRegistry)monitorItem;
//            Exception exception = null;
//            ActionParameters actionParameters = new ActionParameters();

//            try
//            {

//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//            }

//            try
//            {
//                CheckEvents(actionerList, monitorItem, actionParameters, exception);
//            }
//            catch (Exception ex)
//            {

//            }

//            return Task.CompletedTask;
//        }

//        public bool CanCheck(MonitorItem monitorItem)
//        {
//            return monitorItem.MonitorItemType == MonitorItemTypes.Registry;
//        }

//        private void CheckEvents(List<IActioner> actionerList, MonitorItem monitorRegistry, ActionParameters actionParameters, Exception exception)
//        {
//            foreach (EventItem eventItem in monitorRegistry.EventItems)
//            {
//                bool meetsCondition = false;

//                switch (eventItem.EventCondition.Source)
//                {
//                    case EventConditionSource.Exception:
//                        meetsCondition = (exception != null);
//                        break;
//                    case EventConditionSource.NoException:
//                        meetsCondition = (exception == null);
//                        break;                    
//                }
               
//                if (meetsCondition)
//                {
//                    foreach (ActionItem actionItem in eventItem.ActionItems)
//                    {
//                        DoAction(actionerList, monitorRegistry, actionItem, actionParameters);
//                    }
//                }
//            }
//        }

//        private void DoAction(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
//        {
//            foreach (IActioner actioner in actionerList)
//            {
//                if (actioner.CanExecute(actionItem))
//                {
//                    actioner.ExecuteAsync(monitorItem, actionItem, actionParameters);
//                    break;
//                }
//            }
//        }
//    }
//}
