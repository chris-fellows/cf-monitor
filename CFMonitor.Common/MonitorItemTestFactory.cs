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
            items.Add(CreateTestMonitorURL());
            items.Add(CreateTestSQLServiceMonitor());
            items.Add(CreateTestMonitorPing());
            items.Add(CreateTestMonitorSQL());
            items.Add(CreateTestMonitorLocalFile());
            items.Add(CreateTestMonitorProcess());
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

        private static MonitorActiveProcess CreateTestMonitorProcess()
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
                Server = "www.google.co.uk"
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
            ActionEmail actionEmail = new ActionEmail()
            {
                Subject = subject,
                Body = body
            };
            actionEmail.RecipientList.Add(MonitorItemTestFactory.DeveloperEmail);
            actionEmail.Server = "MyEmailServer";
            return actionEmail;
        }
    }
}
