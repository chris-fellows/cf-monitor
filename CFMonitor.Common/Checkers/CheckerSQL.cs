﻿using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using CFUtilities.Databases;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace CFMonitor
{
    /// <summary>
    /// Checks running SQL
    /// </summary>
    public class CheckerSQL : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorSQL monitorSQL = (MonitorSQL)monitorItem;
            Exception exception = null;
            OleDbDatabase database = null;
            OleDbDataReader reader = null;
            ActionParameters actionParameters = new ActionParameters();

            try
            {
                database = new OleDbDatabase(monitorSQL.ConnectionString);
                database.Open();
                string sql = System.IO.File.ReadAllText(monitorSQL.QueryFile);
                reader = database.ExecuteReader(System.Data.CommandType.Text, sql, System.Data.CommandBehavior.Default, null);                
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            try
            {
                // Check events
                actionParameters.Values.Add("Body", "Error checking SQL");
                CheckEvents(actionerList, monitorSQL, actionParameters, exception, reader);
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
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorSQL monitorSQL, ActionParameters actionParameters, Exception exception, OleDbDataReader reader)
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

                /*
                if (eventItem.EventCondition.Source.Equals("OnException"))
                {
                    meetsCondition = (exception != null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnNoException"))
                {
                    meetsCondition = (exception == null);
                }                
                else if (eventItem.EventCondition.Source.Equals("OnRows"))
                {
                    meetsCondition = (reader != null && readerHasRows == true);
                }
                else if (eventItem.EventCondition.Source.EndsWith("OnNoRows"))
                {
                    meetsCondition = (reader != null && readerHasRows == false);
                }
                */

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
            return monitorItem is MonitorSQL;
        }

        private void DoAction(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            foreach (IActioner actioner in actionerList)
            {
                if (actioner.CanAction(actionItem))
                {
                    actioner.DoAction(monitorItem, actionItem, actionParameters);
                    break;
                }
            }
        }
    }
}