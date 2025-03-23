using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks file
    /// 
    /// Examples of use:
    /// - Checking that a scheduled task ran and created a log for today and it contained the text 
    ///   "PROCESSING COMPLETED SUCCESSFULLY".
    /// </summary>
    public class CheckerLocalFile : CheckerBase, IChecker
    {        
        public CheckerLocalFile(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
            IEventItemService eventItemService,
                ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, systemValueTypeService)
        {
     
        }

        public string Name => "Local file";

        //public CheckerTypes CheckerType => CheckerTypes.LocalFile;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, bool testMode)
        {
            return Task.Factory.StartNew(() =>
            {
                var monitorItemOutput = new MonitorItemOutput();

                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return monitorItemOutput;
                }

                var systemValueTypes = _systemValueTypeService.GetAll();

                var svtFileName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_LocalFileFileName);
                var fileNameParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtFileName.Id);

                var svtFindText = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_LocalFileFindText);
                var findTextParam = monitorItem.Parameters.FirstOrDefault(p => p.SystemValueTypeId == svtFindText.Id);

                Exception exception = null;
                var actionItemParameters = new List<ActionItemParameter>();
                FileInfo fileInfo = null;
                bool textFound = false;

                try
                {
                    fileInfo = new FileInfo(fileNameParam.Value);
                    if (fileInfo.Exists && findTextParam != null && !String.IsNullOrEmpty(findTextParam.Value))
                    {
                        textFound = File.ReadAllText(fileNameParam.Value).Contains(findTextParam.Value);
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                try
                {
                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem,  monitorItem, actionItemParameters, exception, fileInfo, textFound))
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
            return monitorItem.MonitorItemType == MonitorItemTypes.LocalFile;
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorFile, List<ActionItemParameter> actionItemParameters, Exception exception, FileInfo fileInfo, bool textFound)
        {                       
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_LocalFileExists:
                        meetsCondition = eventItem.EventCondition.IsValid(fileInfo != null && fileInfo.Exists);
                        break;
                    case SystemValueTypes.ECS_LocalFileTextFound:
                        meetsCondition = eventItem.EventCondition.IsValid(textFound);
                        break;
                }

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorFile, actionItem, actionParameters);
            //    }
            //}            

            return meetsCondition;
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
