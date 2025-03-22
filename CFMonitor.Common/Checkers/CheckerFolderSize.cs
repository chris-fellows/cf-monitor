﻿using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
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
    public class CheckerFolderSize : CheckerBase, IChecker
    {        
        public CheckerFolderSize(IEventItemService eventItemService,
            ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService) {
        {
     
        }

        public string Name => "Folder size";

        //public CheckerTypes CheckerType => CheckerTypes.FolderSize;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            return Task.Factory.StartNew(async () =>
            {
                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return;
                }

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
                    foreach (var eventItem in eventItems)
                    {
                        await CheckEventAsync(eventItem, actionerList, monitorItem, actionParameters, exception, folderSize, systemValueTypes);
                    }
                }
                catch (System.Exception ex)
                {

                }
            });
        }

        private async Task CheckEventAsync(EventItem eventItem, 
                            List<IActioner> actionerList, MonitorItem monitorFolderSize, ActionParameters actionParameters, Exception exception, long? folderSize,
                                List<SystemValueType> systemValueTypes)
        {           
            var svtfolderSizeMaxFolderSize = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_FolderSizeMaxFolderSizeBytes);
            var folderSizeMaxFolderSize = monitorFolderSize.Parameters.First(p => p.SystemValueTypeId == svtfolderSizeMaxFolderSize.Id);
            var maxFolderSizeBytes = Convert.ToDouble(folderSizeMaxFolderSize.Value);
            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_FolderSizeInTolerance:
                        meetsCondition = eventItem.EventCondition.IsValid(folderSize.Value <= maxFolderSizeBytes);
                        break;
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        await ExecuteActionAsync(actionerList, monitorFolderSize, actionItem, actionParameters);
                    }
                }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.FolderSize;
        }

        //private void DoAction(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        //{
        //    foreach (IActioner actioner in actionerList)
        //    {
        //        if (actioner.CanExecute(actionItem))
        //        {
        //            actioner.ExecuteAsync(monitorItem, actionItem, actionParameters);
        //            break;
        //        }
        //    }
        //}
    }
}
