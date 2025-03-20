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
        //private readonly IFileObjectService _fileObjectService;
        private readonly ISystemValueTypeService _systemValueTypeService;

        public MonitorItemSeed1(ISystemValueTypeService systemValueTypeService)
        {          
            _systemValueTypeService = systemValueTypeService;
        }

        public Task<List<MonitorItem>> ReadAllAsync()
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            var items = new List<MonitorItem>();

            items.Add(CreateTestMonitorCPU(systemValueTypes));
            items.Add(CreateTestMonitorFileSize(systemValueTypes));
            items.Add(CreateTestMonitorFolderSize(systemValueTypes));
            items.Add(CreateTestMonitorURL(systemValueTypes));
            items.Add(CreateTestMonitorMemory(systemValueTypes));
            items.Add(CreateTestMonitorNTP(systemValueTypes));
            //items.Add(CreateTestSQLServiceMonitor(systemValueTypes));
            items.Add(CreateTestMonitorPing(systemValueTypes));
            items.Add(CreateTestMonitorSQL(systemValueTypes));
            items.Add(CreateTestMonitorLocalFile(systemValueTypes));
            items.Add(CreateTestMonitorActiveProcess(systemValueTypes));

            return Task.FromResult(items);
        }    

        private static string GetNewMonitorItemID()
        {
            return Guid.NewGuid().ToString();
            //return string.Format("Monitor Item.{0}", Guid.NewGuid().ToString());        
        }

        private MonitorItem CreateTestMonitorSQL(List<SystemValueType> systemValueTypes)
        {
            //var fileObject = _fileObjectService.GetAll().First(fo => fo.Name == "Query1.sql");

            MonitorItem monitorSQL = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Monitor SQL",                
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SQLConnectionString).Id,
                        Value = "Connection String",
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SQLSQL).Id,
                        Value = File.ReadAllText(@"D:\\Temp\\Data\\Query1.sql")
                    }
                }
            };

            // Set schedule
            monitorSQL.MonitorItemSchedule.Times = "60sec";

            // Add event for Status not success
            EventItem eventItem = new EventItem();
            eventItem.EventCondition.Source = EventConditionSources.SQLReturnsRows;  //  "OnRows";
            //eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
            //eventItem.EventCondition.Values.Add(System.Net.NetworkInformation.IPStatus.Success);
            monitorSQL.EventItems.Add(eventItem);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Query returned data", "The query returned data"));
            return monitorSQL;
        }

        private static MonitorItem CreateTestMonitorActiveProcess(List<SystemValueType> systemValueTypes)
        {
            var svtActiveProcessFileName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_ActiveProcessFileName);
            var svtActiveProcessMachineName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_ActiveProcessMachineName);

            MonitorItem monitorProcess = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check Process",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = svtActiveProcessFileName.Id,
                        Value = @"C:\My Data\SomeProcess.exe"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = svtActiveProcessMachineName.Id,
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
            eventItem2.EventCondition.Source = EventConditionSources.ActiveProcessNotRunning;
            monitorProcess.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("Process not running",
                        string.Format("The process {0} is not running", monitorProcess.Parameters.First(p => p.SystemValueTypeId == svtActiveProcessFileName.Id).Value)));
            return monitorProcess;
        }

        private static MonitorItem CreateTestMonitorLocalFile(List<SystemValueType> systemValueTypes)
        {
            var svtLocalFileName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_LocalFileFileName);

            MonitorItem monitorFile = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check Process",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = svtLocalFileName.Id,
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
            eventItem2.EventCondition.Source = EventConditionSources.FileNotExists; //  "OnFileNotExists";
            monitorFile.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("File not found",
                    string.Format("The file {0} was not found", monitorFile.Parameters.First(p => p.SystemValueTypeId == svtLocalFileName.Id).Value)));
            return monitorFile;
        }

        private static MonitorItem CreateTestMonitorPing(List<SystemValueType> systemValueTypes)
        {
            var svtServer = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_PingServer);

            MonitorItem monitorPing = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Ping Google",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = svtServer.Id,
                        Value = "google.co.uk"
                    }
                }                
            };

            // Set schedule
            monitorPing.MonitorItemSchedule.Times = "60sec";

            // Add event for Status not success
            EventItem eventItem = new EventItem();
            eventItem.EventCondition.Source = EventConditionSources.PingReplyStatus; //  "Reply.Status";
            eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
            eventItem.EventCondition.Values.Add(System.Net.NetworkInformation.IPStatus.Success);
            monitorPing.EventItems.Add(eventItem);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Ping failed",
                        string.Format("Ping {0} failed", monitorPing.Parameters.First(p => p.SystemValueTypeId == svtServer.Id).Value)));
            return monitorPing;
        }

        private static MonitorItem CreateTestMonitorURL(List<SystemValueType> systemValueTypes)
        {
            var svtUrl = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLURL);

            MonitorItem monitorURL = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                Name = "Check Google",
                Enabled = true,

                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLMethod).Id,
                        Value = "GET"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType ==SystemValueTypes.MIP_URLPassword).Id,
                        Value = ""
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLProxyName).Id,
                        Value = ""
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType ==SystemValueTypes.MIP_URLProxyPort).Id,
                        Value = "0"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLURL).Id,
                        Value = @"https://www.google.co.uk"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLUsername).Id,
                        Value = ""
                    }
                }
            };

            // Set schedule
            monitorURL.MonitorItemSchedule.Times = "60sec";

            // Add event for StatusCode not OK, send email
            EventItem eventItem = new EventItem();
            eventItem.EventCondition.Source = EventConditionSources.HttpResponseStatusCode;  // "Response.StatusCode";
            eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
            eventItem.EventCondition.Values.Add(System.Net.HttpStatusCode.OK);
            monitorURL.EventItems.Add(eventItem);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Web connection failed",
                        string.Format("Error opening URL {0}", monitorURL.Parameters.First(p => p.SystemValueTypeId == svtUrl.Id).Value)));
            return monitorURL;
        }

        //private static MonitorItem CreateTestSQLServiceMonitor()
        //{
        //    MonitorItem monitorService = new MonitorItem()
        //    {
        //        Id = GetNewMonitorItemID(),
        //        Name = "Check SQL Server",
        //        Parameters = new List<MonitorItemParameter>()
        //        {
        //            new MonitorItemParameter()
        //            {
        //                SystemValueType = SystemValueTypes.MIP_ServiceServiceName,
        //                Value = "SQL Server (SQLEXPRESS)"
        //            },
        //            new MonitorItemParameter()
        //            {
        //                SystemValueType = SystemValueTypes.MIP_ServiceMachineName,
        //                Value = ""
        //            },
        //        },                
        //        Enabled = true
        //    };

        //    // Set schedule
        //    monitorService.MonitorItemSchedule.Times = "60sec";

        //    // Add event for StatusCode not OK, send email
        //    EventItem eventItem = new EventItem();
        //    eventItem.EventCondition.Source = EventConditionSource.ServiceControllerStatus; // "ServiceController.Status";
        //    eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
        //    eventItem.EventCondition.Values.Add(System.ServiceProcess.ServiceControllerStatus.Running);
        //    monitorService.EventItems.Add(eventItem);
        //    eventItem.ActionItems.Add(CreateDefaultActionEmail("SQL Server service not running",
        //                string.Format("Service {0} is not running", monitorService.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_ServiceServiceName).Value)));
        //    return monitorService;
        //}

        private static ActionItem CreateDefaultActionEmail(string subject, string body)
        {
            ActionItem action = new ActionItem()
            {
                //Subject = subject,
                //Body = body
            };
            //action.RecipientList.Add(MonitorItemTestFactory.DeveloperEmail);
            //action.Server = "MyEmailServer";
            return action;
        }

        private static ActionItem CreateDefaultActionLog(string body)
        {
            ActionItem action = new ActionItem()
            {
                //LogFileName = "D:\\Temp\\Logs\\MyLog.txt"
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

        private static MonitorItem CreateTestMonitorFolderSize(List<SystemValueType> systemValueTypes)
        {
            MonitorItem monitorFolderSize = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Monitor folder size",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType ==SystemValueTypes.MIP_FolderSizeFolder).Id,
                        Value ="D:\\Temp"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType ==SystemValueTypes.MIP_FolderSizeMaxFolderSizeBytes).Id,
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
            eventItem2.EventCondition.Source = EventConditionSources.FolderSizeOutsideTolerance;
            monitorFolderSize.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("Folder size above threshold",
                            "The folder size is above the threshold"));

            return monitorFolderSize;
        }

        private static MonitorItem CreateTestMonitorFileSize(List<SystemValueType> systemValueTypes)
        {
            MonitorItem monitorFileSize = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Monitor test log file size",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_FileSizeFile).Id,
                        Value ="D:\\Temp\\Test.log"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType ==SystemValueTypes.MIP_FileSizeMaxFileSizeBytes).Id,
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
            eventItem2.EventCondition.Source = EventConditionSources.FileSizeOutsideTolerance;
            monitorFileSize.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("File size above threshold",
                            "The file size is above the threshold"));

            return monitorFileSize;
        }

        private static MonitorItem CreateTestMonitorCPU(List<SystemValueType> systemValueTypes)
        {
            MonitorItem monitorCPU = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check CPU",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_CPUServer).Id,
                        Value = Environment.MachineName
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
            eventItem2.EventCondition.Source = EventConditionSources.CPUOutsideTolerance;
            monitorCPU.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("CPU above threshold",
                            "The CPU is above the threshold"));

            return monitorCPU;
        }

        private static MonitorItem CreateTestMonitorMemory(List<SystemValueType> systemValueTypes)
        {
            MonitorItem monitorMemory = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
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
            eventItem2.EventCondition.Source = EventConditionSources.MemoryOutsideTolerance;
            monitorMemory.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("Memory use above threshold",
                            "The memory use is above the threshold"));

            return monitorMemory;
        }

        private static MonitorItem CreateTestMonitorNTP(List<SystemValueType> systemValueTypes)
        {
            MonitorItem monitorNTP = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                Enabled = true,
                Name = "Check NTP time",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_NTPServer).Id,
                        Value = Environment.MachineName
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType ==SystemValueTypes.MIP_NTPMaxToleranceSecs).Id,
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
            eventItem2.EventCondition.Source = EventConditionSources.NTPTimeOutsideTolerance;
            monitorNTP.EventItems.Add(eventItem2);
            eventItem2.ActionItems.Add(CreateDefaultActionEmail("Machine time incorrect",
                            "The machine time is different from the NTP server and outside the threshold"));

            return monitorNTP;
        }
    }
}
