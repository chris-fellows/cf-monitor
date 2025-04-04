﻿using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks disk space
    /// </summary>
    public class CheckerDiskSpace : CheckerBase, IChecker
    {        
        public CheckerDiskSpace(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService,
            IEventItemService eventItemService,
              IFileObjectService fileObjectService,
            IPlaceholderService placeholderService,
                                ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService,fileObjectService, placeholderService, systemValueTypeService)
        {
     
        }

        public string Name => "Disk space";

        //public CheckerTypes CheckerType => CheckerTypes.DiskSpace;

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

                var svtDrive = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_DiskSpaceDrive);
                var driveParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtDrive.Id);
                var drive = GetValueWithPlaceholdersReplaced(driveParam);

                Exception exception = null;
                DriveInfo driveInfo = null;             

                try
                {
                    driveInfo = new DriveInfo(drive);
                }
                catch (Exception ex)
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
                    if (driveInfo != null)
                    {
                        monitorItemOutput.ActionItemParameters.Add(new ActionItemParameter()
                        {
                            SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIPC_Message).Id,
                            Value = $"Free space={driveInfo.AvailableFreeSpace} bytes"
                        });
                    }

                    foreach (var eventItem in eventItems)
                    {                        
                        if (IsEventValid(eventItem, monitorItem, exception, driveInfo))
                        {
                            monitorItemOutput.EventItemIdsForAction.Add(eventItem.Id);
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                return monitorItemOutput;
            });
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.DiskSpace;
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorDiskSpace, Exception exception, DriveInfo driveInfo)
        {            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_DiskSpaceAvailableBytes:
                        meetsCondition = eventItem.EventCondition.IsValid(driveInfo.AvailableFreeSpace);
                        break;
                }

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorDiskSpace, actionItem, actionItemParameters);
            //    }
            //}
            //
            return meetsCondition;
        }

        //private void DoAction(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> actionItemParameters)
        //{
        //    foreach (IActioner actioner in actionerList)
        //    {
        //        if (actioner.CanExecute(actionItem))
        //        {
        //            actioner.ExecuteAsync(monitorItem, actionItem, actionItemParameters);
        //            break;
        //        }
        //    }
        //}
    }
}
