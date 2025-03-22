using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks result of file size
    /// </summary>
    public class CheckerFileSize : CheckerBase, IChecker
    {        
        public CheckerFileSize(IEventItemService eventItemService,
                        ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService)
        {
            
        }

        public string Name => "File size";

        //public CheckerTypes CheckerType => CheckerTypes.FileSize;

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
                    foreach (var eventItem in eventItems)
                    {
                        await CheckEventAsync(eventItem, actionerList, monitorItem, actionParameters, exception, fileSize, systemValueTypes);
                    }
                }
                catch (System.Exception ex)
                {

                }
            });
        }

        private async Task CheckEventAsync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorFileSize, ActionParameters actionParameters, Exception exception, double? fileSize,
                                List<SystemValueType> systemValueTypes)
        {            
            var svtFileSizeMailFileSize = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_FileSizeMaxFileSizeBytes);
            var fileSizeMaxFileSize = monitorFileSize.Parameters.First(p => p.SystemValueTypeId == svtFileSizeMailFileSize.Id);            
            var maxFileSizeBytes = Convert.ToDouble(fileSizeMaxFileSize.Value);
            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_FileSizeInTolerance:
                        meetsCondition = eventItem.EventCondition.IsValid(fileSize <= maxFileSizeBytes);
                        break;
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        await ExecuteActionAsync(actionerList, monitorFileSize, actionItem, actionParameters);
                    }
                }            
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.FileSize;
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
