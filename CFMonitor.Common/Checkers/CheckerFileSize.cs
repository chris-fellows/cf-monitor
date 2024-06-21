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
    /// Checks result of file size
    /// </summary>
    public class CheckerFileSize : IChecker
    {
        public string Name => "File size";

        public CheckerTypes CheckerType => CheckerTypes.FileSize;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorFileSize monitorFileSize = (MonitorFileSize)monitorItem;
            Exception exception = null;
            ActionParameters actionParameters = new ActionParameters();
            double? fileSize = null;

            try
            {
                var fileInfo = new FileInfo(monitorFileSize.File);
                fileSize = fileInfo.Length;
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            try
            {
                // Check events
                actionParameters.Values.Add("Body", "Error checking file size");
                CheckEvents(actionerList, monitorFileSize, actionParameters, exception, fileSize);
            }
            catch (System.Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorFileSize monitorFileSize, ActionParameters actionParameters, Exception exception, double? fileSize)
        {
            foreach (EventItem eventItem in monitorFileSize.EventItems)
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
                    case EventConditionSource.FileSizeInTolerance:
                        meetsCondition = fileSize != null && fileSize.Value <= monitorFileSize.MaxFileSizeBytes;
                        break;
                    case EventConditionSource.FileSizeOutsideTolerance:
                        meetsCondition = fileSize != null && fileSize.Value > monitorFileSize.MaxFileSizeBytes;
                        break;
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorFileSize, actionItem, actionParameters);
                    }
                }
            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorFolderSize;
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
