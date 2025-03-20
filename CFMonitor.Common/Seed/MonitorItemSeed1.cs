using CFMonitor.Actioners;
using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class MonitorItemSeed1 : IEntityReader<MonitorItem>
    {
        public Task<List<MonitorItem>> ReadAllAsync()
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

            return Task.FromResult(items);
        }    

        private static string GetNewMonitorItemID()
        {
            return Guid.NewGuid().ToString();
            //return string.Format("Monitor Item.{0}", Guid.NewGuid().ToString());        
        }

        private static MonitorItem CreateTestMonitorSQL()
        {
            MonitorItem monitorSQL = new MonitorItem()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Monitor SQL",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_SQLConnectionString,
                        Value = "Connection String",
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_SQLQuery,
                        Value = @"C:\My Data\Query1.sql"
                    }
                }
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

        private static MonitorItem CreateTestMonitorActiveProcess()
        {
            MonitorItem monitorProcess = new MonitorItem()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check Process",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_ActiveProcessFileName,
                        Value = @"C:\My Data\SomeProcess.exe"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_ActiveProcessMachineName
                        Value = @"MY_MACHINE"
                    },
                }
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

        private static MonitorItem CreateTestMonitorLocalFile()
        {
            MonitorItem monitorFile = new MonitorItem()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check Process",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_LocalFileFileName,
                        Value = @"C:\My Data\SomeProcess.exe"
                    }
                }                
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

        private static MonitorItem CreateTestMonitorPing()
        {
            MonitorItem monitorPing = new MonitorItem()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Ping Google",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_PingServer,
                        Value = "google.co.uk"
                    }
                }                
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

        private static MonitorItem CreateTestMonitorURL()
        {
            MonitorItem monitorURL = new MonitorItem()
            {
                ID = GetNewMonitorItemID(),
                Name = "Check Google",
                Enabled = true,

                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_URLMethod,
                        Value = "GET"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_URLPassword,
                        Value = ""
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_URLProxyName,
                        Value = ""
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_URLProxyPort,
                        Value = "0"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_URLURL,
                        Value = @"https://www.google.co.uk"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_URLUsername,
                        Value = ""
                    }
                }
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

        private static MonitorItem CreateTestSQLServiceMonitor()
        {
            MonitorItem monitorService = new MonitorItem()
            {
                ID = GetNewMonitorItemID(),
                Name = "Check SQL Server",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_ServiceServiceName,
                        Value = "SQL Server (SQLEXPRESS)"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_ServiceMachineName
                        Value = ""
                    },
                },                
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

        private static ActionItem CreateDefaultActionEmail(string subject, string body)
        {
            ActionItem action = new ActionItem()
            {
                Subject = subject,
                Body = body
            };
            action.RecipientList.Add(MonitorItemTestFactory.DeveloperEmail);
            action.Server = "MyEmailServer";
            return action;
        }

        private static ActionItem CreateDefaultActionLog(string body)
        {
            ActionItem action = new ActionItem()
            {
                LogFileName = "D:\\Temp\\Logs\\MyLog.txt"
            };
            return action;
        }

        private static ActionItem CreateDefaultActionConsole(string body)
        {
            ActionItem action = new ActionItem()
            {

            };
            return action;
        }

        private static MonitorItem CreateTestMonitorFolderSize()
        {
            MonitorItem monitorFolderSize = new MonitorItem()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Monitor folder size",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_FolderSizeFolder,
                        Value ="D:\\Temp"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_FolderSizeMaxFolderSizeBytes,
                        Value = "5000"
                    }
                }
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

        private static MonitorItem CreateTestMonitorFileSize()
        {
            MonitorItem monitorFileSize = new MonitorItem()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Monitor test log file size",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_FileSizeFile,
                        Value ="D:\\Temp\\Test.log"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_FileSizeMaxFileSizeBytes,
                        Value = "5000"
                    }
                }
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

        private static MonitorItem CreateTestMonitorCPU()
        {
            MonitorItem monitorCPU = new MonitorItem()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check CPU",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_CPUServer,
                        Value =Environment.MachineName
                    }
                }                
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

        private static MonitorItem CreateTestMonitorMemory()
        {
            MonitorItem monitorMemory = new MonitorItem()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check memory",
                Parameters = new List<MonitorItemParameter>()
                {

                }
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

        private static MonitorItem CreateTestMonitorNTP()
        {
            MonitorItem monitorNTP = new MonitorItem()
            {
                ID = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check NTP time",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_NTPServer,
                        Value = Environment.MachineName
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_NTPMaxToleranceSecs,
                        Value = "30"
                    }
                }                
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
