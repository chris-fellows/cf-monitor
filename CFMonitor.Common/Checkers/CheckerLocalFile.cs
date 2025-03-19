﻿using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
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
    public class CheckerLocalFile : IChecker
    {
        public string Name => "Local file";

        public CheckerTypes CheckerType => CheckerTypes.LocalFile;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            //MonitorLocalFile monitorFile = (MonitorLocalFile)monitorItem;

            var fileNameParam = monitorItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_LocalFileFileName);
            var findTextParam = monitorItem.Parameters.FirstOrDefault(p => p.SystemValueType == SystemValueTypes.MIP_LocalFileFindText);

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
                CheckEvents(actionerList, monitorItem, actionParameters, exception, fileInfo, textFound);
            }
            catch (Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.LocalFile;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorItem monitorFile, ActionParameters actionParameters, Exception exception, FileInfo fileInfo, bool textFound)
        {           
            foreach (EventItem eventItem in monitorFile.EventItems)
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
                    case EventConditionSource.FileExists:
                        meetsCondition = fileInfo != null && fileInfo.Exists;
                        break;
                    case EventConditionSource.FileNotExists:
                        meetsCondition = (fileInfo == null) || (fileInfo != null && !fileInfo.Exists);
                        break;
                    case EventConditionSource.TextFoundInFile:
                        meetsCondition = textFound;
                        break;
                    case EventConditionSource.TextNotFoundInFile:
                        meetsCondition = !textFound;
                        break;
                }
             
                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorFile, actionItem, actionParameters);
                    }
                }
            }
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
