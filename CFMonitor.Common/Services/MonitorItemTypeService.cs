using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System.Drawing;

namespace CFMonitor.Services
{
    public class MonitorItemTypeService : IMonitorItemTypeService
    {
        private readonly ISystemValueTypeService _systemValueTypeService;

        public MonitorItemTypeService(ISystemValueTypeService systemValueTypeService)
        {
            _systemValueTypeService = systemValueTypeService;
        }

        public List<MonitorItemType> GetAll()
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            var list = new List<MonitorItemType>();
            list.Add(new MonitorItemType()
            {                 
                Name = "Active Process",               
                Description = "Checks an active process",
                ItemType = MonitorItemTypes.ActiveProcess,
                //CheckerType = CheckerTypes.ActiveProcess,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_ActiveProcessRunning
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_ActiveProcessFileName).Id,
                        Value = "File.exe"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_ActiveProcessMachineName).Id
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
            });
            list.Add(new MonitorItemType()
            {
                Name = "CPU",
                Description = "Checks CPU",
                ItemType = MonitorItemTypes.CPU,
                //CheckerType = CheckerTypes.CPU,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_CPUInTolerance
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {

                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"

            });
            list.Add(new MonitorItemType()
            {
                Name = "DCHP",
                Description = "Checks that DHCP is working",
                ItemType = MonitorItemTypes.DHCP,
                //CheckerType = CheckerTypes.DHCP,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception                    
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {

                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
            });
            list.Add(new MonitorItemType()
            {
                Name = "Disk space",
                Description = "Checks disk space on particular device",
                ItemType = MonitorItemTypes.DiskSpace,
                //CheckerType = CheckerTypes.DiskSpace,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_DiskSpaceAvailableBytes
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_DiskSpaceDrive).Id,
                        Value = "D:\\"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
            });
            list.Add(new MonitorItemType()
            {
                Name = "DNS",
                Description = "Checks that DNS is working",
                ItemType = MonitorItemTypes.DNS,
               // CheckerType = CheckerTypes.DNS,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_DNSHost).Id,
                        Value = "Host"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
            });
            list.Add(new MonitorItemType()
            {
                Name = "File size",
                Description = "Checks size of file",
                ItemType = MonitorItemTypes.FileSize,
                //CheckerType = CheckerTypes.FileSize,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_FileSizeInTolerance,
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_FileSizeFile).Id,
                        Value = "File.txt"                        
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_FileSizeMaxFileSizeBytes).Id,
                        Value = "1000000"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"

                //CreateMonitorItem = () => new MonitorFileSize()
                //{
                //    ID = Guid.NewGuid().ToString(),
                //    Name = "File size [New]",
                //    Enabled = true,
                //    EventItems = new List<EventItem>(),
                //    MonitorItemSchedule = new MonitorItemSchedule()
                //}
            });
            list.Add(new MonitorItemType()
            {
                Name = "Folder size",
                Description = "Checks size of folder",
                ItemType = MonitorItemTypes.FolderSize,
                //CheckerType = CheckerTypes.FolderSize,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_FolderSizeInTolerance
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_FolderSizeFolder).Id,
                        Value = "Folder"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_FolderSizeMaxFolderSizeBytes).Id,
                         Value = "1000000"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorFolderSize()
                //{
                //    ID = Guid.NewGuid().ToString(),
                //    Name = "Folder size [New]",
                //    Enabled = true,
                //    EventItems = new List<EventItem>(),
                //    MonitorItemSchedule = new MonitorItemSchedule()
                //}
            });
            list.Add(new MonitorItemType()
            {
                Name = "IMAP",
                Description = "Checks connection to IMAP server",
                ItemType = MonitorItemTypes.IMAP,
                //CheckerType = CheckerTypes.IMAP,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_IMAPConnected
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {

                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorIMAP()
                //{
                //    ID = Guid.NewGuid().ToString(),
                //    Name = "IMAP [New]",
                //    Enabled = true,
                //    EventItems = new List<EventItem>(),
                //    MonitorItemSchedule = new MonitorItemSchedule()
                //}
            });
            list.Add(new MonitorItemType()
            {
                Name = "Local file",
                Description = "Checks local file exists and optionally contains specific text",
                ItemType = MonitorItemTypes.LocalFile,
                //CheckerType = CheckerTypes.LocalFile,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_LocalFileExists,
                    SystemValueTypes.ECS_LocalFileTextFound
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType ==SystemValueTypes.MIP_LocalFileFileName).Id,
                        Value = "File.txt"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_LocalFileFindText).Id,
                        Value = "FindThis"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorLocalFile() { ID = Guid.NewGuid().ToString(), 
                //            Name = "Local file [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "JSON",
                Description = "Checks JSON file",
                ItemType = MonitorItemTypes.JSON,
                //CheckerType = CheckerTypes.JSON,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,                    
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {

                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorJSON() { ID = Guid.NewGuid().ToString(), 
                //            Name = "JSON [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "LDAP",
                Description = "Checks that LDAP is working",
                ItemType = MonitorItemTypes.LDAP,
                //CheckerType = CheckerTypes.LDAP,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {

                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorLDAP() { ID = Guid.NewGuid().ToString(), 
                //            Name = "LDAP [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Memory",
                Description = "Checks local memory use",
                ItemType = MonitorItemTypes.Memory,
                //CheckerType = CheckerTypes.Memory,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_MemoryInTolerance
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {

                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorMemory() { ID = Guid.NewGuid().ToString(), 
                //            Name = "Memory [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Time",
                Description = "Checks time with remote server",
                ItemType = MonitorItemTypes.Time,
                //CheckerType = CheckerTypes.NTP,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_NTPTimeInTolerance
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_TimeServer).Id,
                        Value = "HTTP"       // HTTP/NIST/NTP
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_TimeServer).Id,
                        Value = "https://www.google.co.uk"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_TimeMaxToleranceSecs).Id,
                        Value = "60"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorNTP()
                //{
                //    ID = Guid.NewGuid().ToString(),
                //    Name = "NTP [New]",
                //    Enabled = true,
                //    EventItems = new List<EventItem>(),
                //    MonitorItemSchedule = new MonitorItemSchedule()
                //}
            });
            list.Add(new MonitorItemType()
            {
                Name = "Ping",
                Description = "Pings endpoint",
                ItemType = MonitorItemTypes.Ping,
                //CheckerType = CheckerTypes.Ping,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_PingReplyStatus
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_PingServer).Id,
                        Value = "Server"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorPing() { ID = Guid.NewGuid().ToString(), 
                //            Name = "Ping [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "POP",
                Description = "Checks connection to POP server",
                ItemType = MonitorItemTypes.POP,
                //CheckerType = CheckerTypes.POP,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_POPConnected
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {

                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorPOP()
                //{
                //    ID = Guid.NewGuid().ToString(),
                //    Name = "POP [New]",
                //    Enabled = true,
                //    EventItems = new List<EventItem>(),
                //    MonitorItemSchedule = new MonitorItemSchedule()
                //}
            });
            //list.Add(new MonitorItemType()
            //{
            //    Name = "Registry",
            //    Description = "Checks for particular registry setting(s)",
            //    ItemType = MonitorItemTypes.Registry,
            //    CheckerType = CheckerTypes.Registry,
            //    DefaultParameters = new List<MonitorItemParameter>()
            //    {
            //        new MonitorItemParameter()
            //        {
            //            SystemValueType= SystemValueTypes.MIP_RegistryKey,
            //            Value = "Key"
            //        },
            //        new MonitorItemParameter()
            //        {
            //            SystemValueType = SystemValueTypes.MIP_RegistryValue,
            //            Value= "Value"
            //        }
            //    }
            //    //CreateMonitorItem = () => new MonitorRegistry() { ID = Guid.NewGuid().ToString(), 
            //    //            Name = "Registry [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            //});
            list.Add(new MonitorItemType()
            {
                Name = "REST API",
                Description = "Checks REST API returns expected response",
                ItemType = MonitorItemTypes.REST,
                //CheckerType = CheckerTypes.REST,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_HTTPResponseStatusCode
                },
                DefaultParameters = new List<MonitorItemParameter>()
                { 
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_RESTURL).Id,
                        Value = "http://myrestapi.com/test"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorREST() { ID = Guid.NewGuid().ToString(), 
                //            Name = "REST API [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Run Process",
                Description = "Runs process and checks the exit code",
                ItemType = MonitorItemTypes.RunProcess,
                //CheckerType = CheckerTypes.RunProcess,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_RunProcessExitCode
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId =systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_RunProcessFileName).Id,
                        Value = "File.exe"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorRunProcess() { ID = Guid.NewGuid().ToString(), 
                //            Name = "Run Process [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            //list.Add(new MonitorItemType()
            //{
            //    Name = "Service",
            //    Description = "Checks Windows service status",
            //    ItemType = MonitorItemTypes.Service,
            //    CheckerType = CheckerTypes.Service,
            //    DefaultParameters = new List<MonitorItemParameter>()
            //    {
            //        new MonitorItemParameter()
            //        {
            //            SystemValueType = SystemValueTypes.MIP_ServiceMachineName,
            //            Value = "Machine"
            //        },
            //        new MonitorItemParameter()
            //        {
            //            SystemValueType = SystemValueTypes.MIP_ServiceServiceName,
            //            Value = "My Service"
            //        }
            //    }
            //    //CreateMonitorItem = () => new MonitorService() { ID = Guid.NewGuid().ToString(), 
            //    //            Name = "Service [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            //});
            list.Add(new MonitorItemType()
            {
                Name = "SMTP",
                Description = "Checks SMTP connection",
                ItemType = MonitorItemTypes.SMTP,
                //CheckerType = CheckerTypes.SMTP,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception                    
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_SMTPPort).Id,
                        Value = "100"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId =systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_SMTPServer).Id,
                        Value = "Server"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorSMTP() { ID = Guid.NewGuid().ToString(), 
                //            Name = "SMTP [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "SOAP",
                Description = "Checks SOAP API returns expected response",
                ItemType = MonitorItemTypes.SOAP,
                //CheckerType = CheckerTypes.SOAP,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_HTTPResponseStatusCode
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_SOAPURL).Id,
                        Value = "http://soapurl/test"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_SOAPXML).Id,
                        Value = "test"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorSOAP() { ID = Guid.NewGuid().ToString(), 
                //            Name = "SOAP [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "TCP or UDP socket",
                Description = "Checks TCP or UDP socket",
                ItemType = MonitorItemTypes.Socket,
                //CheckerType = CheckerTypes.Socket,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_SocketConnected
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_SocketHost).Id,
                        Value = "Host"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_SocketPort).Id,
                        Value = "1000"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_SocketProtocol).Id,
                        Value = "TCP"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorSocket() { ID = Guid.NewGuid().ToString(), 
                //            Name = "TCP or UDP socket [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "SQL query",
                Description = "Checks results of SQL query",
                ItemType = MonitorItemTypes.SQL,
                //CheckerType = CheckerTypes.SQL,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_SQLReturnsRows
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_SQLConnectionString).Id,
                        Value= "Connection String"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId  = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_SQLSQL).Id,
                        Value = "select * from Test"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorSQL() { ID = Guid.NewGuid().ToString(), 
                //            Name = "SQL Query [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "HTTP/HTTPS endpoint",
                Description = "Checks HTTP/HTTPS endpoint returns expected response",
                ItemType = MonitorItemTypes.URL,
                //CheckerType = CheckerTypes.URL,
                EventConditionValueTypes = new List<SystemValueTypes>()
                {
                    SystemValueTypes.ECS_Exception,
                    SystemValueTypes.ECS_HTTPResponseStatusCode
                },
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_URLMethod).Id,
                        Value = "GET"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId =systemValueTypes.First(t => t.ValueType ==  SystemValueTypes.MIP_URLPassword).Id,
                        Value = "password"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId= systemValueTypes.First(t => t.ValueType ==SystemValueTypes.MIP_URLProxyName).Id,
                        Value = "Proxy"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId =systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_URLProxyPort).Id,
                        Value = "1000"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId =systemValueTypes.First(t => t.ValueType == SystemValueTypes.MIP_URLURL).Id,
                        Value = "http://google.co.uk/test"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(t => t.ValueType ==SystemValueTypes.MIP_URLUsername).Id,
                        Value = "username"
                    }
                },
                Color = Color.Blue.ToArgb().ToString(),
                ImageSource = "monitor_item_type.png"
                //CreateMonitorItem = () => new MonitorURL() { ID = Guid.NewGuid().ToString(), 
                //            Name = "HTTP/HTTPS endpoint [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            return list;
        }
    }
}
