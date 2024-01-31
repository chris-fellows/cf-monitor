using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CFMonitor
{
    /// <summary>
    /// Checks disk space
    /// </summary>
    public class CheckerDiskSpace : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorDiskSpace monitorDiskSpace = (MonitorDiskSpace)monitorItem;
            Exception exception = null;
            DriveInfo driveInfo = null;            
            ActionParameters actionParameters = new CFMonitor.ActionParameters();

            try
            {
                driveInfo = new DriveInfo(monitorDiskSpace.Name);                             
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            try
            {
                CheckEvents(actionerList, monitorDiskSpace, actionParameters, exception, driveInfo);
            }
            catch (Exception ex)
            {

            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorDiskSpace;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorDiskSpace monitorDiskSpace, ActionParameters actionParameters, Exception exception, DriveInfo driveInfo)
        {
            foreach (EventItem eventItem in monitorDiskSpace.EventItems)
            {
                bool meetsCondition = false;
                if (eventItem.EventCondition.Source.Equals("OnException"))
                {
                    meetsCondition = (exception != null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnNoException"))
                {
                    meetsCondition = (exception == null);
                }
                else if (eventItem.EventCondition.Source.Equals("Drive.AvailableFreeSpace"))
                {
                    meetsCondition = eventItem.EventCondition.Evaluate(driveInfo.AvailableFreeSpace);
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorDiskSpace, actionItem, actionParameters);
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
