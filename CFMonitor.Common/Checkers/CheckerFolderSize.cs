using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using CFUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks result of folder size
    /// </summary>
    public class CheckerFolderSize : IChecker
    {
        public string Name => "Folder size";

        public CheckerTypes CheckerType => CheckerTypes.FolderSize;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            MonitorFolderSize monitorFolderSize = (MonitorFolderSize)monitorItem;
            Exception exception = null;
            ActionParameters actionParameters = new ActionParameters();
            long? folderSize = null;

            try
            {
                if (Directory.Exists(monitorFolderSize.Folder))
                {
                    folderSize = IOUtilities.GetDirectorySize(monitorFolderSize.Folder);
                }
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            try
            {
                // Check events
                actionParameters.Values.Add("Body", "Error checking folder size");
                CheckEvents(actionerList, monitorFolderSize, actionParameters, exception, folderSize);
            }
            catch (System.Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorFolderSize monitorFolderSize, ActionParameters actionParameters, Exception exception, long? folderSize)
        {
            foreach (EventItem eventItem in monitorFolderSize.EventItems)
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
                    case EventConditionSource.FolderSizeInTolerance:
                        meetsCondition = folderSize != null && folderSize.Value <= monitorFolderSize.MaxFolderSizeBytes;
                        break;
                    case EventConditionSource.FolderSizeOutsideTolerance:
                        meetsCondition = folderSize != null && folderSize.Value > monitorFolderSize.MaxFolderSizeBytes;
                        break;
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorFolderSize, actionItem, actionParameters);
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
