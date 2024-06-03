using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks disk space
    /// </summary>
    public class CheckerDiskSpace : IChecker
    {
        public string Name => "Disk space";

        public CheckerTypes CheckerType => CheckerTypes.DiskSpace;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorDiskSpace monitorDiskSpace = (MonitorDiskSpace)monitorItem;
            Exception exception = null;
            DriveInfo driveInfo = null;            
            ActionParameters actionParameters = new ActionParameters();

            try
            {
                driveInfo = new DriveInfo(monitorDiskSpace.Name);                             
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            try
            {
                CheckEvents(actionerList, monitorDiskSpace, actionParameters, exception, driveInfo);
            }
            catch (Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorDiskSpace;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorDiskSpace monitorDiskSpace, ActionParameters actionParameters, Exception exception, DriveInfo driveInfo)
        {
            foreach (EventItem eventItem in monitorDiskSpace.EventItems)
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
                    case EventConditionSource.DriveAvailableFreeSpace:
                        meetsCondition = eventItem.EventCondition.Evaluate(driveInfo.AvailableFreeSpace);
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
                else if (eventItem.EventCondition.Source.Equals("Drive.AvailableFreeSpace"))
                {
                    meetsCondition = eventItem.EventCondition.Evaluate(driveInfo.AvailableFreeSpace);
                }
                */

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorDiskSpace, actionItem, actionParameters);
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
