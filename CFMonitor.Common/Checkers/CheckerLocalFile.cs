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
            MonitorLocalFile monitorFile = (MonitorLocalFile)monitorItem;
            Exception exception = null;       
            ActionParameters actionParameters = new ActionParameters();
            FileInfo fileInfo = null;
            bool textFound = false;

            try
            {                
                fileInfo = new FileInfo(monitorFile.FileName);
                if (fileInfo.Exists && !String.IsNullOrEmpty(monitorFile.FindText))
                {
                    textFound = File.ReadAllText(monitorFile.FileName).Contains(monitorFile.FindText);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            try
            {
                CheckEvents(actionerList, monitorFile, actionParameters, exception, fileInfo, textFound);
            }
            catch (Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorLocalFile;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorLocalFile monitorFile, ActionParameters actionParameters, Exception exception, FileInfo fileInfo, bool textFound)
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
