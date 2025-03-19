using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using CFUtilities.Databases;
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
    public class CheckerSQL : IChecker
    {
        public string Name => "SQL query";

        public CheckerTypes CheckerType => CheckerTypes.SQL;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            //MonitorSQL monitorSQL = (MonitorSQL)monitorItem;
            var connectionStringParam = monitorItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_SQLConnectionString);
            var queryParam = monitorItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_SQLQuery);

            Exception exception = null;
            OleDbDatabase database = null;
            OleDbDataReader reader = null;
            ActionParameters actionParameters = new ActionParameters();

            try
            {
                database = new OleDbDatabase(connectionStringParam.Value);
                database.Open();
                string sql = System.IO.File.ReadAllText(queryParam.Value);
                reader = database.ExecuteReader(System.Data.CommandType.Text, sql, System.Data.CommandBehavior.Default, null);                
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            try
            {
                // Check events
                actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking SQL");
                CheckEvents(actionerList, monitorItem, actionParameters, exception, reader);
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
                if (database != null && database.IsOpen)
                {
                    database.Close();
                }
            }

            return Task.CompletedTask;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorItem monitorSQL, ActionParameters actionParameters, Exception exception, OleDbDataReader reader)
        {
            bool readerHasRows = (reader != null && reader.Read());

            foreach (EventItem eventItem in monitorSQL.EventItems)
            {
                bool meetsCondition = false;

                switch (eventItem.EventCondition.Source)
                {
                    case EventConditionSource.Exception:
                        meetsCondition = (exception != null);
                        break;
                    case EventConditionSource.NoException:
                        meetsCondition = (exception == null);
                        break;
                    case EventConditionSource.SQLReturnsRows:
                        meetsCondition = (reader != null && readerHasRows == true);
                        break;
                    case EventConditionSource.SQLReturnsNoRows:
                        meetsCondition = (reader != null && readerHasRows == false);
                        break;
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorSQL, actionItem, actionParameters);
                    }
                }
            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.SQL;
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
