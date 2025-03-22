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
        public CheckerLocalFile(IEventItemService eventItemService,
                ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService)
        {
     
        }

        public string Name => "Local file";

        //public CheckerTypes CheckerType => CheckerTypes.LocalFile;

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

                var svtFileName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_LocalFileFileName);
                var fileNameParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtFileName.Id);

                var svtFindText = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_LocalFileFindText);
                var findTextParam = monitorItem.Parameters.FirstOrDefault(p => p.SystemValueTypeId == svtFindText.Id);

                Exception exception = null;
                ActionParameters actionParameters = new ActionParameters();
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
                        await CheckEventAsync(eventItem, actionerList, monitorItem, actionParameters, exception, fileInfo, textFound);
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.LocalFile;
        }

        private async Task CheckEventAsync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorFile, ActionParameters actionParameters, Exception exception, FileInfo fileInfo, bool textFound)
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
             
                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        await ExecuteActionAsync(actionerList, monitorFile, actionItem, actionParameters);
                    }
                }            
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
