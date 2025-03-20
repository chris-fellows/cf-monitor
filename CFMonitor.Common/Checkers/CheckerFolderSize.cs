using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using CFUtilities.Utilities;
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
        private readonly ISystemValueTypeService _systemValueTypeService;

        public CheckerFolderSize(ISystemValueTypeService systemValueTypeService)
        {
            _systemValueTypeService = systemValueTypeService;
        }

        public string Name => "Folder size";

        public CheckerTypes CheckerType => CheckerTypes.FolderSize;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            var svtFolder = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_FolderSizeFolder);
            var folderParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtFolder.Id);

            Exception exception = null;
            ActionParameters actionParameters = new ActionParameters();
            long? folderSize = null;

            try
            {
                if (Directory.Exists(folderParam.Value))
                {
                    folderSize = IOUtilities.GetDirectorySize(folderParam.Value);
                }
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            try
            {
                // Check events
                actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking folder size");
                CheckEvents(actionerList, monitorItem, actionParameters, exception, folderSize, systemValueTypes);
            }
            catch (System.Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorItem monitorFolderSize, ActionParameters actionParameters, Exception exception, long? folderSize,
                                List<SystemValueType> systemValueTypes)
        {
            var svtfolderSizeMaxFolderSize = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_FolderSizeMaxFolderSizeBytes);
            var folderSizeMaxFolderSize = monitorFolderSize.Parameters.First(p => p.SystemValueTypeId == svtfolderSizeMaxFolderSize.Id);
            var maxFolderSizeBytes = Convert.ToDouble(folderSizeMaxFolderSize.Value);

            foreach (EventItem eventItem in monitorFolderSize.EventItems)
            {
                bool meetsCondition = false;

                switch (eventItem.EventCondition.Source)
                {
                    case EventConditionSources.Exception:
                        meetsCondition = (exception != null);
                        break;
                    case EventConditionSources.NoException:
                        meetsCondition = (exception == null);
                        break;
                    case EventConditionSources.FolderSizeInTolerance:
                        meetsCondition = folderSize != null && folderSize.Value <= maxFolderSizeBytes;
                        break;
                    case EventConditionSources.FolderSizeOutsideTolerance:
                        meetsCondition = folderSize != null && folderSize.Value > maxFolderSizeBytes;
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
            return monitorItem.MonitorItemType == MonitorItemTypes.FolderSize;
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
