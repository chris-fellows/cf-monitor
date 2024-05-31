using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.IO;

namespace CFMonitor
{
    /// <summary>
    /// Checks file
    /// </summary>
    public class CheckerFile : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorFile monitorFile = (MonitorFile)monitorItem;
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
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorFile;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorFile monitorFile, ActionParameters actionParameters, Exception exception, FileInfo fileInfo, bool textFound)
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

                /*
                if (eventItem.EventCondition.Source.Equals("OnException"))
                {
                    meetsCondition = (exception != null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnNoException"))
                {
                    meetsCondition = (exception == null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnFileExists"))
                {
                    meetsCondition = fileInfo != null && fileInfo.Exists;
                }
                else if (eventItem.EventCondition.Source.Equals("OnFileNotExists"))
                {
                    meetsCondition = (fileInfo == null) || (fileInfo != null && !fileInfo.Exists);
                }
                else if (eventItem.EventCondition.Source.Equals("OnTextFound"))
                {
                    meetsCondition = textFound;
                }
                else if (eventItem.EventCondition.Source.Equals("OnTextNotFound"))
                {
                    meetsCondition = !textFound;
                }
                */

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
                if (actioner.CanAction(actionItem))
                {
                    actioner.DoAction(monitorItem, actionItem, actionParameters);
                    break;
                }
            }          
        }
    }
}
