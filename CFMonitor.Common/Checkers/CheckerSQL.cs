using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities;
using CFUtilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks running SQL
    /// 
    /// Examples of use:
    /// - Check SQL query that returns a list of issues from the data.
    /// </summary>
    public class CheckerSQL : CheckerBase, IChecker
    {        
        public CheckerSQL(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
                    IEventItemService eventItemService,
                      IFileObjectService fileObjectService,
                    IPlaceholderService placeholderService,
                        ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, fileObjectService, placeholderService, systemValueTypeService)
        {
            
        }

        public string Name => "SQL query";

        //public CheckerTypes CheckerType => CheckerTypes.SQL;

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

                var svtConnectionString = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SQLConnectionString);
                var connectionStringParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtConnectionString.Id);
                var connectionString = GetValueWithPlaceholdersReplaced(connectionStringParam);

                var svtQuery = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SQLSQL);
                var queryParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtQuery.Id);
                var query = GetValueWithPlaceholdersReplaced(queryParam);

                var svtFileObjectId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_RunProcessFileObjectId);
                var fileObjectIdParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtFileObjectId.Id);
                var fileObjectId = GetValueWithPlaceholdersReplaced(fileObjectIdParam);     // Shouldn't use placeholders

                var queryToRun = query;     // Default

                // Get file object if set
                FileObject? fileObject = String.IsNullOrEmpty(fileObjectId) ? null : _fileObjectService.GetByIdAsync(fileObjectId).Result;
                if (fileObject != null)
                {
                    queryToRun = System.Text.Encoding.UTF8.GetString(fileObject.Content);
                }

                Exception exception = null;
                
                OleDbConnection? connection = null;
                OleDbDataReader? reader = null;                                

                try
                {
                    connection = new OleDbConnection(connectionString);
                    connection.Open();

                    var command = new OleDbCommand(query, connection);
                    reader = command.ExecuteReader();
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
                    // Check events                    
                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem, monitorItem, exception, reader.HasRows))
                        {
                            monitorItemOutput.EventItemIdsForAction.Add(eventItem.Id);
                        }
                    }
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }

                    if (connection != null && connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }

                return monitorItemOutput;            
            });
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorSQL, Exception exception, bool hasRows)
        {                        
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_SQLReturnsRows:
                        meetsCondition = eventItem.EventCondition.IsValid(hasRows);
                        break;
                }

                //if (meetsCondition)
                //{
                //    foreach (ActionItem actionItem in eventItem.ActionItems)
                //    {
                //        await ExecuteActionAsync(actionerList, monitorSQL, actionItem, actionParameters);
                //    }
                //}

            return meetsCondition;
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.SQL;
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
