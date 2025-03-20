﻿using CFMonitor.Enums;
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
        private readonly ISystemValueTypeService _systemValueTypeService;

        public CheckerFileSize(ISystemValueTypeService systemValueTypeService)
        {
            _systemValueTypeService = systemValueTypeService;
        }

        public string Name => "File size";

        public CheckerTypes CheckerType => CheckerTypes.FileSize;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            var svtFileSize = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_FileSizeFile);
            var fileSizeParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtFileSize.Id);            

            Exception exception = null;
            ActionParameters actionParameters = new ActionParameters();
            double? fileSize = null;

            try
            {
                if (File.Exists(fileSizeParam.Value))
                {
                    var fileInfo = new FileInfo(fileSizeParam.Value);
                    fileSize = fileInfo.Length;
                }
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            try
            {
                // Check events
                actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking file size");
                CheckEvents(actionerList, monitorItem, actionParameters, exception, fileSize, systemValueTypes);
            }
            catch (System.Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorItem monitorFileSize, ActionParameters actionParameters, Exception exception, double? fileSize,
                                List<SystemValueType> systemValueTypes)
        {
            var svtFileSizeMailFileSize = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_FileSizeMaxFileSizeBytes);
            var fileSizeMaxFileSize = monitorFileSize.Parameters.First(p => p.SystemValueTypeId == svtFileSizeMailFileSize.Id);            
            var maxFileSizeBytes = Convert.ToDouble(fileSizeMaxFileSize.Value);

            foreach (EventItem eventItem in monitorFileSize.EventItems)
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
                    case EventConditionSources.FileSizeInTolerance:
                        meetsCondition = fileSize != null && fileSize.Value <= maxFileSizeBytes;
                        break;
                    case EventConditionSources.FileSizeOutsideTolerance:
                        meetsCondition = fileSize != null && fileSize.Value > maxFileSizeBytes;
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
            return monitorItem.MonitorItemType == MonitorItemTypes.FileSize;
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
