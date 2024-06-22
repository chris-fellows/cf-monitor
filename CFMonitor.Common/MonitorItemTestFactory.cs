using CFMonitor.Enums;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;

namespace CFMonitor
{
    /// <summary>
    /// Factory for creating monitor items for testing
    /// </summary>
    public class MonitorItemTestFactory
    {
        public static string DeveloperEmail = "chrismfellows@hotmail.co.uk";

        public static List<MonitorItem> Create()
        {
            var items = new List<MonitorItem>();
            items.Add(CreateTestMonitorCPU());
            items.Add(CreateTestMonitorFileSize());
            items.Add(CreateTestMonitorFolderSize());
            items.Add(CreateTestMonitorURL());
            items.Add(CreateTestMonitorMemory());
            items.Add(CreateTestMonitorNTP());
            items.Add(CreateTestSQLServiceMonitor());
            items.Add(CreateTestMonitorPing());
            items.Add(CreateTestMonitorSQL());
            items.Add(CreateTestMonitorLocalFile());
            items.Add(CreateTestMonitorActiveProcess());
            return items;
        }

        private static string GetNewMonitorItemID()
        {
            return Guid.NewGuid().ToString();
            //return string.Format("Monitor Item.{0}", Guid.NewGuid().ToString());        
        }

        private static MonitorSQL CreateTestMonitorSQL()
        {
            MonitorSQL monitorSQL = new MonitorSQL()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Monitor SQL",
                ConnectionString = "Connection String",
                QueryFile = @"C:\My Data\Query1.sql"
            };

            // Set schedule
            monitorSQL.MonitorItemSchedule.Times = "60sec";

            // Add event for Status not success
            EventItem eventItem = new EventItem();
            eventItem.EventCondition.Source = EventConditionSource.SQLReturnsRows;  //  "OnRows";
            //eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
            //eventItem.EventCondition.Values.Add(System.Net.NetworkInformation.IPStatus.Success);
            monitorSQL.EventItems.Add(eventItem);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Query returned data", "The query returned data"));
            return monitorSQL;
        }

        private static MonitorActiveProcess CreateTestMonitorActiveProcess()
        {
            MonitorActiveProcess monitorProcess = new MonitorActiveProcess()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check Process",
                FileName = @"C:\My Data\SomeProcess.exe"
            };

            // Set schedule
            monitorProcess.MonitorItemSchedule.Times = "60sec";

            // Add event for Status not success
            //EventItem eventItem1 = new EventItem();
            //eventItem1.EventCondition.Source = "OnProcessRunning";
            //monitorProcess.EventItems.Add(eventItem1);
            //eventItem1.ActionItems.Add(CreateDefaultActionEmail("Process running"));

            EventItem eventItem2 = new EventItem();
            eventItem2.EventCondition.Source = EventConditionSource.ActiveProcessNotRunning;
            monitorProcess.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("Process not running",
                        string.Format("The process {0} is not running", monitorProcess.FileName)));
            return monitorProcess;
        }

        private static MonitorLocalFile CreateTestMonitorLocalFile()
        {
            MonitorLocalFile monitorFile = new MonitorLocalFile()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check Process",
                FileName = @"C:\My Data\SomeProcess.exe"
            };

            // Set schedule
            monitorFile.MonitorItemSchedule.Times = "60sec";

            // Add event for Status not success
            //EventItem eventItem1 = new EventItem();
            //eventItem1.EventCondition.Source = "OnProcessRunning";
            //monitorProcess.EventItems.Add(eventItem1);
            //eventItem1.ActionItems.Add(CreateDefaultActionEmail("Process running"));

            EventItem eventItem2 = new EventItem();
            eventItem2.EventCondition.Source = EventConditionSource.FileNotExists; //  "OnFileNotExists";
            monitorFile.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("File not found", 
                    string.Format("The file {0} was not found", monitorFile.FileName)));
            return monitorFile;
        }

        private static MonitorPing CreateTestMonitorPing()
        {
            MonitorPing monitorPing = new MonitorPing()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Ping Google",
                Server = "google.co.uk"
            };

            // Set schedule
            monitorPing.MonitorItemSchedule.Times = "60sec";

            // Add event for Status not success
            EventItem eventItem = new EventItem();
            eventItem.EventCondition.Source = EventConditionSource.PingReplyStatus; //  "Reply.Status";
            eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
            eventItem.EventCondition.Values.Add(System.Net.NetworkInformation.IPStatus.Success);
            monitorPing.EventItems.Add(eventItem);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Ping failed", 
                        string.Format("Ping {0} failed", monitorPing.Server)));
            return monitorPing;
        }

        private static MonitorURL CreateTestMonitorURL()
        {
            MonitorURL monitorURL = new MonitorURL()
            {
                ID = GetNewMonitorItemID(),
                Name = "Check Google",
                Enabled = true,
                URL = @"https://www.google.co.uk",
                Method = "GET",                
                ProxyName = "",
                ProxyPort = 0,
                UserName = "",
                Password = ""
            };

            // Set schedule
            monitorURL.MonitorItemSchedule.Times = "60sec";

            // Add event for StatusCode not OK, send email
            EventItem eventItem = new EventItem();
            eventItem.EventCondition.Source = EventConditionSource.HttpResponseStatusCode;  // "Response.StatusCode";
            eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
            eventItem.EventCondition.Values.Add(System.Net.HttpStatusCode.OK);
            monitorURL.EventItems.Add(eventItem);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Web connection failed", 
                        string.Format("Error opening URL {0}", monitorURL.URL)));                        
            return monitorURL;
        }

        private static MonitorService CreateTestSQLServiceMonitor()
        {
            MonitorService monitorService = new MonitorService()
            {
                ID = GetNewMonitorItemID(),
                Name = "Check SQL Server",
                ServiceName = "SQL Server (SQLEXPRESS)",
                Enabled = true                
            };

            // Set schedule
            monitorService.MonitorItemSchedule.Times = "60sec";

            // Add event for StatusCode not OK, send email
            EventItem eventItem = new EventItem();
            eventItem.EventCondition.Source = EventConditionSource.ServiceControllerStatus; // "ServiceController.Status";
            eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
            eventItem.EventCondition.Values.Add(System.ServiceProcess.ServiceControllerStatus.Running);
            monitorService.EventItems.Add(eventItem);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("SQL Server service not running",
                        string.Format("Service {0} is not running", monitorService.ServiceName)));            
            return monitorService;
        }

        private static ActionEmail CreateDefaultActionEmail(string subject, string body)
        {
            ActionEmail action = new ActionEmail()
            {
                Subject = subject,
                Body = body
            };
            action.RecipientList.Add(MonitorItemTestFactory.DeveloperEmail);
            action.Server = "MyEmailServer";
            return action;
        }

        private static ActionLog CreateDefaultActionLog(string body)
        {
            ActionLog action = new ActionLog()
            {
                LogFileName = "D:\\Temp\\Logs\\MyLog.txt"
            };            
            return action;
        }

        private static ActionConsole CreateDefaultActionConsole(string body)
        {
            ActionConsole action = new ActionConsole()
            {
                
            };
            return action;
        }

        private static MonitorFolderSize CreateTestMonitorFolderSize()
        {
            MonitorFolderSize monitorFolderSize = new MonitorFolderSize()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Monitor folder size",
                Folder = "D:\\Temp",
                MaxFolderSizeBytes = 50000
            };

            // Set schedule
            monitorFolderSize.MonitorItemSchedule.Times = "60sec";

            // Add event for Status not success
            //EventItem eventItem1 = new EventItem();
            //eventItem1.EventCondition.Source = "OnProcessRunning";
            //monitorProcess.EventItems.Add(eventItem1);
            //eventItem1.ActionItems.Add(CreateDefaultActionEmail("Process running"));

            EventItem eventItem2 = new EventItem();
            eventItem2.EventCondition.Source = EventConditionSource.FolderSizeOutsideTolerance;
            monitorFolderSize.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("Folder size above threshold",
                            "The folder size is above the threshold"));

            return monitorFolderSize;
        }

        private static MonitorFileSize CreateTestMonitorFileSize()
        {
            MonitorFileSize monitorFileSize = new MonitorFileSize()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Monitor test log file size",
                File = "D:\\Temp\\Test.log",
                MaxFileSizeBytes = 50000
            };

            // Set schedule
            monitorFileSize.MonitorItemSchedule.Times = "60sec";

            // Add event for Status not success
            //EventItem eventItem1 = new EventItem();
            //eventItem1.EventCondition.Source = "OnProcessRunning";
            //monitorProcess.EventItems.Add(eventItem1);
            //eventItem1.ActionItems.Add(CreateDefaultActionEmail("Process running"));

            EventItem eventItem2 = new EventItem();
            eventItem2.EventCondition.Source = EventConditionSource.FileSizeOutsideTolerance;
            monitorFileSize.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("File size above threshold",
                            "The file size is above the threshold"));

            return monitorFileSize;
        }

        private static MonitorCPU CreateTestMonitorCPU()
        {
            MonitorCPU monitorCPU = new MonitorCPU()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check CPU",
                Server = Environment.MachineName               
            };

            // Set schedule
            monitorCPU.MonitorItemSchedule.Times = "60sec";

            // Add event for Status not success
            //EventItem eventItem1 = new EventItem();
            //eventItem1.EventCondition.Source = "OnProcessRunning";
            //monitorProcess.EventItems.Add(eventItem1);
            //eventItem1.ActionItems.Add(CreateDefaultActionEmail("Process running"));

            EventItem eventItem2 = new EventItem();
            eventItem2.EventCondition.Source = EventConditionSource.CPUOutsideTolerance;
            monitorCPU.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("CPU above threshold",
                            "The CPU is above the threshold"));
                        
            return monitorCPU;
        }

        private static MonitorMemory CreateTestMonitorMemory()
        {
            MonitorMemory monitorMemory = new MonitorMemory()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check memory"                
            };

            // Set schedule
            monitorMemory.MonitorItemSchedule.Times = "60sec";

            // Add event for Status not success
            //EventItem eventItem1 = new EventItem();
            //eventItem1.EventCondition.Source = "OnProcessRunning";
            //monitorProcess.EventItems.Add(eventItem1);
            //eventItem1.ActionItems.Add(CreateDefaultActionEmail("Process running"));

            EventItem eventItem2 = new EventItem();
            eventItem2.EventCondition.Source = EventConditionSource.MemoryOutsideTolerance;
            monitorMemory.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("Memory use above threshold",
                            "The memory use is above the threshold"));

            return monitorMemory;
        }

        private static MonitorNTP CreateTestMonitorNTP()
        {
            MonitorNTP monitorNTP = new MonitorNTP()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check NTP time",
                MaxToleranceSecs = 30
            };

            // Set schedule
            monitorNTP.MonitorItemSchedule.Times = "60sec";

            // Add event for Status not success
            //EventItem eventItem1 = new EventItem();
            //eventItem1.EventCondition.Source = "OnProcessRunning";
            //monitorProcess.EventItems.Add(eventItem1);
            //eventItem1.ActionItems.Add(CreateDefaultActionEmail("Process running"));

            EventItem eventItem2 = new EventItem();
            eventItem2.EventCondition.Source = EventConditionSource.NTPTimeOutsideTolerance;
            monitorNTP.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("Machine time incorrect",
                            "The machine time is different from the NTP server and outside the threshold"));

            return monitorNTP;
        }
    }
}
