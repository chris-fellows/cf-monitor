using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
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
        public CheckerFolderSize(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService,
            IEventItemService eventItemService,
              IFileObjectService fileObjectService,
            IPlaceholderService placeholderService,
            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, fileObjectService, placeholderService, systemValueTypeService)
        {
     
        }

        public string Name => "Folder size";

        //public CheckerTypes CheckerType => CheckerTypes.FolderSize;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, CheckerConfig checkerConfig)
        {
            return Task.Factory.StartNew(() =>
            {
                SetPlaceholders(monitorAgent, monitorItem, checkerConfig);

                var monitorItemOutput = new MonitorItemOutput();

                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return monitorItemOutput;
                }

                var systemValueTypes = _systemValueTypeService.GetAll();

                var svtFolder = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_FolderSizeFolder);
                var folderParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtFolder.Id);
                var folder = GetValueWithPlaceholdersReplaced(folderParam);

                Exception exception = null;
                var actionItemParameters = new List<ActionItemParameter>();
                long? folderSize = null;

                try
                {
                    if (Directory.Exists(folder))
                    {
                        folderSize = IOUtilities.GetDirectorySize(folder);
                    }
                }
                catch (System.Exception ex)
                {
                    exception = ex;
                }

                try
                {
                    // Check events
                    //actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking folder size");
                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem, monitorItem, actionItemParameters, exception, folderSize, systemValueTypes))
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

        private bool IsEventValid(EventItem eventItem, 
                            MonitorItem monitorFolderSize, List<ActionItemParameter> actionItemParameters, Exception exception, long? folderSize,
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

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorFolderSize, actionItem, actionParameters);
            //    }
            //}

            return meetsCondition;
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
