﻿using CFMonitor.EntityReader;
using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class MonitorItemSeed1 : IEntityReader<MonitorItem>
    {
        private readonly IFileObjectService _fileObjectService;
        private readonly IMonitorAgentService _monitorAgentService;
        private readonly ISystemValueTypeService _systemValueTypeService;

        public MonitorItemSeed1(IFileObjectService fileObjectService,
                                IMonitorAgentService monitorAgentService,
                                ISystemValueTypeService systemValueTypeService)
        {
            _fileObjectService = fileObjectService;
            _monitorAgentService = monitorAgentService;
            _systemValueTypeService = systemValueTypeService;
        }

        public IEnumerable<MonitorItem> Read()
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            var items = new List<MonitorItem>();

            //items.Add(CreateTestMonitorCPU(systemValueTypes));
            //items.Add(CreateTestMonitorFileSize(systemValueTypes));
            //items.Add(CreateTestMonitorFolderSize(systemValueTypes));
            items.Add(CreateTestMonitorURL(systemValueTypes));
            //items.Add(CreateTestMonitorMemory(systemValueTypes));
            items.Add(CreateTestMonitorTime(systemValueTypes));
            items.Add(CreateTestMonitorPing(systemValueTypes));
            //items.Add(CreateTestMonitorSQLErrors(systemValueTypes));
            //items.Add(CreateTestMonitorLocalFile(systemValueTypes));
            items.Add(CreateTestMonitorMSEdgeActiveProcess(systemValueTypes));
            items.Add(CreateTestMonitorMSOfficeInstalled(systemValueTypes));

            // Run all monitor items on every Monitor Agent
            var monitorAgents = _monitorAgentService.GetAll();
            foreach(var item in items)
            {
                item.MonitorAgentIds = monitorAgents.Select(mi => mi.Id).ToList();
            }

            return items;
        }    

        private static string GetNewMonitorItemID()
        {
            return Guid.NewGuid().ToString();
            //return string.Format("Monitor Item.{0}", Guid.NewGuid().ToString());        
        }

        private MonitorItem CreateTestMonitorSQLErrors(List<SystemValueType> systemValueTypes)
        {
            var fileObject = _fileObjectService.GetAll().First(fi => fi.Name.Equals("Errors.sql"));

            MonitorItem monitorSQL = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                MonitorItemType = MonitorItemTypes.SQL,
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
                        Value = ""          // "SELECT * FROM Errors"
                    },
                     new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SQLSQLFileObjectId).Id,
                        Value = fileObject.Id
                    }
                }
            };

            // Set schedule
            monitorSQL.MonitorItemSchedule.Times = "900sec";

            //// Add event for Status not success
            //EventItem eventItem = new EventItem();
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_SQLReturnsRows;
            //eventItem.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem.EventCondition.Values.Add(true);
            //monitorSQL.EventItems.Add(eventItem);
            //eventItem.ActionItems.Add(CreateDefaultActionEmail("Query returned data", "The query returned data"));
            return monitorSQL;
        }

        private static MonitorItem CreateTestMonitorMSEdgeActiveProcess(List<SystemValueType> systemValueTypes)
        {
            var svtActiveProcessFileName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_ActiveProcessFileName);
            var svtActiveProcessMachineName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_ActiveProcessMachineName);

            MonitorItem monitorProcess = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                MonitorItemType = MonitorItemTypes.ActiveProcess,
                Enabled = true,
                Name = "Check MS Edge Active Process",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = svtActiveProcessFileName.Id,
                        Value = "msedge.exe"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = svtActiveProcessMachineName.Id,
                        Value = Environment.MachineName
                    },
                }
            };

            // Set schedule
            monitorProcess.MonitorItemSchedule.Times = "60sec";           

            //EventItem eventItem2 = new EventItem();
            //eventItem2.EventCondition.SourceValueType = SystemValueTypes.ECS_ActiveProcessRunning;
            //eventItem2.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem2.EventCondition.Values.Add(false);
            //monitorProcess.EventItems.Add(eventItem2);
            //eventItem2.ActionItems.Add(CreateDefaultActionEmail("Process not running",
            //            string.Format("The process {0} is not running", monitorProcess.Parameters.First(p => p.SystemValueTypeId == svtActiveProcessFileName.Id).Value)));
            return monitorProcess;
        }

        private static MonitorItem CreateTestMonitorLocalFile(List<SystemValueType> systemValueTypes)
        {
            var svtLocalFileName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_LocalFileFileName);

            MonitorItem monitorFile = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                MonitorItemType = MonitorItemTypes.LocalFile,
                Enabled = true,
                Name = "Check Log File Exists",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = svtLocalFileName.Id,
                        Value = @"C:\My Data\LogFile-20250325.txt"
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

            //EventItem eventItem2 = new EventItem();
            //eventItem2.EventCondition.SourceValueType = SystemValueTypes.ECS_LocalFileExists;
            //eventItem2.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem2.EventCondition.Values.Add(false);
            //monitorFile.EventItems.Add(eventItem2);
            //eventItem2.ActionItems.Add(CreateDefaultActionEmail("File not found",
            //        string.Format("The file {0} was not found", monitorFile.Parameters.First(p => p.SystemValueTypeId == svtLocalFileName.Id).Value)));
            return monitorFile;
        }

        private static MonitorItem CreateTestMonitorPing(List<SystemValueType> systemValueTypes)
        {
            var svtServer = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_PingServer);

            MonitorItem monitorPing = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                MonitorItemType = MonitorItemTypes.Ping,
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

            //// Add event for Status not success
            //EventItem eventItem = new EventItem();
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_PingReplyStatus;
            //eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
            //eventItem.EventCondition.Values.Add(System.Net.NetworkInformation.IPStatus.Success);
            //monitorPing.EventItems.Add(eventItem);
            //eventItem.ActionItems.Add(CreateDefaultActionEmail("Ping failed",
            //            string.Format("Ping {0} failed", monitorPing.Parameters.First(p => p.SystemValueTypeId == svtServer.Id).Value)));
            return monitorPing;
        }

        private static MonitorItem CreateTestMonitorURL(List<SystemValueType> systemValueTypes)
        {
            var svtUrl = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLURL);

            MonitorItem monitorURL = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                MonitorItemType = MonitorItemTypes.URL,
                Name = "Check Google Website",
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
            monitorURL.MonitorItemSchedule.Times = "300sec";

            //// Add event for StatusCode not OK, send email
            //EventItem eventItem = new EventItem();
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_HTTPResponseStatusCode;
            //eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
            //eventItem.EventCondition.Values.Add(System.Net.HttpStatusCode.OK);
            //monitorURL.EventItems.Add(eventItem);
            //eventItem.ActionItems.Add(CreateDefaultActionEmail("Web connection failed",
            //            string.Format("Error opening URL {0}", monitorURL.Parameters.First(p => p.SystemValueTypeId == svtUrl.Id).Value)));
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

        private static MonitorItem CreateTestMonitorFolderSize(List<SystemValueType> systemValueTypes)
        {
            MonitorItem monitorFolderSize = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                MonitorItemType = MonitorItemTypes.FolderSize,
                Enabled = true,
                Name = "Check Temp Folder Size",
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
          
            //EventItem eventItem2 = new EventItem();
            //eventItem2.EventCondition.SourceValueType = SystemValueTypes.ECS_FolderSizeInTolerance;
            //eventItem2.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem2.EventCondition.Values.Add(false);
            //monitorFolderSize.EventItems.Add(eventItem2);
            //eventItem2.ActionItems.Add(CreateDefaultActionEmail("Folder size above threshold",
            //                "The folder size is above the threshold"));

            return monitorFolderSize;
        }

        private static MonitorItem CreateTestMonitorFileSize(List<SystemValueType> systemValueTypes)
        {
            MonitorItem monitorFileSize = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                MonitorItemType = MonitorItemTypes.FileSize,
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

            //EventItem eventItem2 = new EventItem();
            //eventItem2.EventCondition.SourceValueType = SystemValueTypes.ECS_FileSizeInTolerance;
            //eventItem2.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem2.EventCondition.Values.Add(false);
            //monitorFileSize.EventItems.Add(eventItem2);
            //eventItem2.ActionItems.Add(CreateDefaultActionEmail("File size above threshold",
            //                "The file size is above the threshold"));

            return monitorFileSize;
        }

        private static MonitorItem CreateTestMonitorCPU(List<SystemValueType> systemValueTypes)
        {
            MonitorItem monitorCPU = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                MonitorItemType= MonitorItemTypes.CPU,
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
            
            //EventItem eventItem2 = new EventItem();
            //eventItem2.EventCondition.SourceValueType = SystemValueTypes.ECS_CPUInTolerance;
            //eventItem2.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem2.EventCondition.Values.Add(false);
            //monitorCPU.EventItems.Add(eventItem2);
            //eventItem2.ActionItems.Add(CreateDefaultActionEmail("CPU above threshold",
            //                "The CPU is above the threshold"));

            return monitorCPU;
        }

        private static MonitorItem CreateTestMonitorMemory(List<SystemValueType> systemValueTypes)
        {
            MonitorItem monitorMemory = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                MonitorItemType = MonitorItemTypes.Memory,
                Enabled = true,
                Name = "Check Memory",
                Parameters = new List<MonitorItemParameter>()
                {

                }
            };

            // Set schedule
            monitorMemory.MonitorItemSchedule.Times = "60sec";
            
            //EventItem eventItem2 = new EventItem();
            //eventItem2.EventCondition.SourceValueType = SystemValueTypes.ECS_MemoryInTolerance;
            //eventItem2.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem2.EventCondition.Values.Add(false);
            //monitorMemory.EventItems.Add(eventItem2);
            //eventItem2.ActionItems.Add(CreateDefaultActionEmail("Memory use above threshold",
            //                "The memory use is above the threshold"));

            return monitorMemory;
        }

        private static MonitorItem CreateTestMonitorTime(List<SystemValueType> systemValueTypes)
        {
            MonitorItem monitorNTP = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                MonitorItemType = MonitorItemTypes.Time,
                Enabled = true,
                Name = "Check Time",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_TimeServerType).Id,
                        Value = "HTTP"      // HTTP/NIST/NTP
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_TimeServer).Id,
                        Value = "https://www.google.co.uk"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType ==SystemValueTypes.MIP_TimeMaxToleranceSecs).Id,
                        Value = "30"
                    }
                }                
            };

            // Set schedule
            monitorNTP.MonitorItemSchedule.Times = "300sec";

            // Add event for Status not success
            //EventItem eventItem1 = new EventItem();
            //eventItem1.EventCondition.Source = "OnProcessRunning";
            //monitorProcess.EventItems.Add(eventItem1);
            //eventItem1.ActionItems.Add(CreateDefaultActionEmail("Process running"));

            //EventItem eventItem2 = new EventItem();
            //eventItem2.EventCondition.SourceValueType = SystemValueTypes.ECS_NTPTimeInTolerance;
            //eventItem2.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem2.EventCondition.Values.Add(false);
            //monitorNTP.EventItems.Add(eventItem2);
            //eventItem2.ActionItems.Add(CreateDefaultActionEmail("Machine time incorrect",
            //                "The machine time is different from the NTP server and outside the threshold"));

            return monitorNTP;
        }

        private MonitorItem CreateTestMonitorMSOfficeInstalled(List<SystemValueType> systemValueTypes)
        {
            var fileObject = _fileObjectService.GetAll().First(fi => fi.Name.Equals("Check MS Office installed.ps1"));

            MonitorItem monitorItem = new MonitorItem()
            {
                Id = GetNewMonitorItemID(),
                MonitorItemType = MonitorItemTypes.RunProcess,
                Enabled = true,
                Name = "Check MS Office installed",
                Parameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_RunProcessFileName).Id,
                        Value = ""     // "Check MS Office installed.ps1"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_RunProcessFileObjectId).Id,
                        Value = fileObject.Id
                    },
                }
            };

            // Set schedule
            monitorItem.MonitorItemSchedule.Times = "900sec";
      
            return monitorItem;
        }
    }
}
