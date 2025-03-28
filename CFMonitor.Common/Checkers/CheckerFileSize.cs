using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
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
        public CheckerFileSize(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService,
            IEventItemService eventItemService,
              IFileObjectService fileObjectService,
            IPlaceholderService placeholderService,
            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, fileObjectService, placeholderService, systemValueTypeService)
        {
            
        }

        public string Name => "File size";

        //public CheckerTypes CheckerType => CheckerTypes.FileSize;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, CheckerConfig checkerConfig)
        {
            return Task.Factory.StartNew(() =>
            {
                SetPlaceholders(monitorAgent, monitorItem, checkerConfig);

                var monitorItemOutput = new MonitorItemOutput()
                {
                    Id = Guid.NewGuid().ToString(),
                    ActionItemParameters = new(),
                    CheckedDateTime = DateTime.UtcNow,
                    EventItemIdsForAction = new(),
                    MonitorAgentId = monitorAgent.Id,
                    MonitorItemId = monitorItem.Id,
                };

                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return monitorItemOutput;
                }

                var systemValueTypes = _systemValueTypeService.GetAll();

                var svtFileSize = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_FileSizeFile);
                var fileSizeParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtFileSize.Id);
                var file = GetValueWithPlaceholdersReplaced(fileSizeParam);

                Exception exception = null;                
                double? fileSize = null;

                try
                {
                    if (File.Exists(file))
                    {
                        var fileInfo = new FileInfo(file);
                        fileSize = fileInfo.Length;
                    }
                }
                catch (System.Exception ex)
                {
                    exception = ex;

                    monitorItemOutput.ActionItemParameters.Add(new ActionItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIPC_ErrorMessage).Id,
                        Value = ex.Message
                    });
                }

                try
                {
                    if (fileSize != null)
                    {
                        monitorItemOutput.ActionItemParameters.Add(new ActionItemParameter()
                        {
                            SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIPC_Message).Id,
                            Value = $"File size={fileSize} bytes"
                        });
                    }

                    // Check events                   
                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem, monitorItem, exception, fileSize, systemValueTypes))
                        {
                            monitorItemOutput.EventItemIdsForAction.Add(eventItem.Id);
                        }
                    }
                }
                catch (System.Exception ex)
                {

                }

                return monitorItemOutput;
            });
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorFileSize, Exception exception, double? fileSize,
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

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorFileSize, actionItem, actionParameters);
            //    }
            //}
            //

            return meetsCondition;
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
