﻿using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading.Tasks;

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
                    IPlaceholderService placeholderService,
                        ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, placeholderService, systemValueTypeService)
        {
            
        }

        public string Name => "SQL query";

        //public CheckerTypes CheckerType => CheckerTypes.SQL;

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

                ////MonitorSQL monitorSQL = (MonitorSQL)monitorItem;
                //var connectionStringParam = monitorItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_SQLConnectionString);                
                //var connectionString = GetValueWithPlaceholdersReplaced(connectionStringParam);

                //var queryParam = monitorItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_SQLQuery);
                // var query = GetValueWithPlaceholdersReplaced(queryParam)

                //Exception exception = null;
                //OleDbDatabase database = null;
                //OleDbDataReader reader = null;
                //ActionParameters actionParameters = new ActionParameters();

                //try
                //{
                //    database = new OleDbDatabase(connectionString);
                //    database.Open();
                //    string sql = System.IO.File.ReadAllText(query);
                //    reader = database.ExecuteReader(System.Data.CommandType.Text, sql, System.Data.CommandBehavior.Default, null);                
                //}
                //catch (System.Exception ex)
                //{
                //    exception = ex;
                //}

                //try
                //{
                //    // Check events
                //    actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking SQL");
                //    foreach(var eventItem in eventItems)
                //    {
                //          if (IsEventValid(eventItem, monitorItem, actionItemParameters, exception, reader))
                //          {
                //              monitorItemOutput.EventItemIdsForAction(eventItem.Id);
                //          }
                //     }
                //}
                //catch (System.Exception ex)
                //{

                //}
                //finally
                //{
                //    if (reader != null && !reader.IsClosed)
                //    {
                //        reader.Close();
                //    }
                //    if (database != null && database.IsOpen)
                //    {
                //        database.Close();
                //    }
                //}

                return monitorItemOutput;
            });
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorSQL, List<ActionItemParameter> actionItemParameters, Exception exception)      //, OleDbDataReader reader)
        {            
            bool readerHasRows = true;  // (reader != null && reader.Read());
            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_SQLReturnsRows:
                        meetsCondition = eventItem.EventCondition.IsValid(readerHasRows);
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
