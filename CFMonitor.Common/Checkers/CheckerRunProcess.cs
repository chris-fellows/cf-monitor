using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities;
using CFUtilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks result of running a process (Checks exit code)
    /// </summary>
    public class CheckerRunProcess : CheckerBase, IChecker
    {
        
        public CheckerRunProcess(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
            IEventItemService eventItemService,
              IFileObjectService fileObjectService,
            IPlaceholderService placeholderService,
            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, fileObjectService, placeholderService, systemValueTypeService)
        {
            
        }

        public string Name => "Run Process";

        //public CheckerTypes CheckerType => CheckerTypes.RunProcess;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, CheckerConfig checkerConfig)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var disposableSession = new DisposableActionsSession())
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

                    var svtRunProcessFileName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_RunProcessFileName);
                    var runProcessFileNameParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtRunProcessFileName.Id);
                    var runProcessFileName = GetValueWithPlaceholdersReplaced(runProcessFileNameParam);

                    var svtRunProcessFileObjectId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_RunProcessFileObjectId);
                    var runProcessFileObjectIdParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtRunProcessFileObjectId.Id);
                    var runProcessFileObjectId = GetValueWithPlaceholdersReplaced(runProcessFileObjectIdParam);     // Shouldn't use placeholders

                    // Default file to run, may override with file object
                    var fileToRun = runProcessFileName;

                    // Get file object if set
                    FileObject? fileObject = String.IsNullOrEmpty(runProcessFileObjectId) ? null : _fileObjectService.GetByIdAsync(runProcessFileObjectId).Result;

                    // If using file object then write to temp folder and run it from there
                    if (fileObject != null)
                    {
                        fileToRun = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}{Path.GetExtension(fileObject.Name)}");
                        File.WriteAllBytes(fileToRun, fileObject.Content);

                        disposableSession.AddAction(() => File.Delete(fileToRun));  // Clean up
                    }

                    //MonitorActiveProcess monitorProcess = (MonitorActiveProcess)monitorItem;
                    Exception exception = null;                
                    int? exitCode = null;

                    try
                    {                        
                        using (var process = new Process())
                        {
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.CreateNoWindow = true;
                            process.StartInfo.FileName = fileToRun;
                            process.Start();
                            process.WaitForExit();

                            exitCode = process.ExitCode;
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
                        if (exitCode != null)
                        {
                            monitorItemOutput.ActionItemParameters.Add(new ActionItemParameter()
                            {
                                SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIPC_Message).Id,
                                Value = $"Exit code={exitCode}"
                            });
                        }

                        // Check events
                        //actionParameters.Values.Add(ActionParameterTypes.Body, "Error running process");
                        foreach (var eventItem in eventItems)
                        {
                            if (IsEventValid(eventItem, monitorItem, exception, exitCode))
                            {
                                monitorItemOutput.EventItemIdsForAction.Add(eventItem.Id);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }

                    return monitorItemOutput;
                }
            });
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorProcess, Exception exception, int? exitCode)
        {            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_RunProcessExitCode:
                        meetsCondition = eventItem.EventCondition.IsValid(exitCode);
                        break;                 
                }

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorProcess, actionItem, actionParameters);
            //    }
            //}
            //
            return meetsCondition;
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.RunProcess;                 
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
