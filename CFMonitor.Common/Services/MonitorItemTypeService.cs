using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace CFMonitor.Services
{
    public class MonitorItemTypeService : IMonitorItemTypeService
    {
        public List<MonitorItemType> GetAll()
        {
            var list = new List<MonitorItemType>();
            list.Add(new MonitorItemType()
            {
                Name = "Active Process",
                Description = "Checks an active process",
                ItemType = MonitorItemTypes.ActiveProcess,
                CheckerType = CheckerTypes.ActiveProcess,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_ActiveProcessFileName,
                        Value = "File.exe"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_ActiveProcessMachineName
                    }
                }
                //CreateMonitorItem = () => new MonitorActiveProcess() { ID = Guid.NewGuid().ToString(), 
                //            Name = "Active Process [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "CPU",
                Description = "Checks CPU",
                ItemType = MonitorItemTypes.CPU,
                CheckerType = CheckerTypes.CPU,
                DefaultParameters = new List<MonitorItemParameter>()
                {

                }                    
                //CreateMonitorItem = () => new MonitorCPU()
                //{
                //    ID = Guid.NewGuid().ToString(),
                //    Name = "CPU [New]",                   
                //    Enabled = true,
                //    EventItems = new List<EventItem>(),
                //    MonitorItemSchedule = new MonitorItemSchedule()
                //}
            });
            list.Add(new MonitorItemType()
            {
                Name = "DCHP",
                Description = "Checks that DHCP is working",
                ItemType = MonitorItemTypes.DHCP,
                CheckerType = CheckerTypes.DHCP,
                DefaultParameters = new List<MonitorItemParameter>()
                {

                }
                //CreateMonitorItem = () => new MonitorDHCP() { ID = Guid.NewGuid().ToString(), 
                //            Name = "DHCP [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Disk space",
                Description = "Checks disk space on particular device",
                ItemType = MonitorItemTypes.DiskSpace,
                CheckerType = CheckerTypes.DiskSpace,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_DiskSpaceDrive,
                        Value = "D:\\"
                    }
                }
                //CreateMonitorItem = () => new MonitorDiskSpace() { ID = Guid.NewGuid().ToString(),
                //            Name = "Disk space [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "DNS",
                Description = "Checks that DNS is working",
                ItemType = MonitorItemTypes.DNS,
                CheckerType = CheckerTypes.DNS,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_DNSHost,
                        Value = "Host"
                    }
                }
                //CreateMonitorItem = () => new MonitorDNS() { ID = Guid.NewGuid().ToString(), 
                //            Name = "DNS [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "File size",
                Description = "Checks size of file",
                ItemType = MonitorItemTypes.FileSize,
                CheckerType = CheckerTypes.FileSize,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_FileSizeFile,
                        Value = "File.txt"                        
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_FileSizeMaxFileSizeBytes,
                        Value = "1000000"
                    }
                }

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
                CheckerType = CheckerTypes.FolderSize,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_FolderSizeFolder,
                        Value = "Folder"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_FolderSizeMaxFolderSizeBytes,
                         Value = "1000000"
                    }
                }
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
                CheckerType = CheckerTypes.IMAP,
                DefaultParameters = new List<MonitorItemParameter>()
                {

                }
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
                CheckerType = CheckerTypes.LocalFile,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_LocalFileFileName,
                        Value = "File.txt"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_LocalFileFindText,
                        Value = "FindThis"
                    }
                }
                //CreateMonitorItem = () => new MonitorLocalFile() { ID = Guid.NewGuid().ToString(), 
                //            Name = "Local file [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "JSON",
                Description = "Checks JSON file",
                ItemType = MonitorItemTypes.JSON,
                CheckerType = CheckerTypes.JSON,
                DefaultParameters = new List<MonitorItemParameter>()
                {

                }
                //CreateMonitorItem = () => new MonitorJSON() { ID = Guid.NewGuid().ToString(), 
                //            Name = "JSON [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "LDAP",
                Description = "Checks that LDAP is working",
                ItemType = MonitorItemTypes.LDAP,
                CheckerType = CheckerTypes.LDAP,
                DefaultParameters = new List<MonitorItemParameter>()
                {

                }
                //CreateMonitorItem = () => new MonitorLDAP() { ID = Guid.NewGuid().ToString(), 
                //            Name = "LDAP [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Memory",
                Description = "Checks local memory use",
                ItemType = MonitorItemTypes.Memory,
                CheckerType = CheckerTypes.Memory,
                DefaultParameters = new List<MonitorItemParameter>()
                {

                }
                //CreateMonitorItem = () => new MonitorMemory() { ID = Guid.NewGuid().ToString(), 
                //            Name = "Memory [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "NTP time",
                Description = "Checks time with NTP server",
                ItemType = MonitorItemTypes.NTP,
                CheckerType = CheckerTypes.NTP,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_NTPServer,
                        Value = "Server"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_NTPMaxToleranceSecs,
                        Value = "60"
                    }
                }
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
                CheckerType = CheckerTypes.Ping,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_PingServer,
                        Value = "Server"
                    }
                }
                //CreateMonitorItem = () => new MonitorPing() { ID = Guid.NewGuid().ToString(), 
                //            Name = "Ping [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "POP",
                Description = "Checks connection to POP server",
                ItemType = MonitorItemTypes.POP,
                CheckerType = CheckerTypes.POP,
                DefaultParameters = new List<MonitorItemParameter>()
                {

                }
                //CreateMonitorItem = () => new MonitorPOP()
                //{
                //    ID = Guid.NewGuid().ToString(),
                //    Name = "POP [New]",
                //    Enabled = true,
                //    EventItems = new List<EventItem>(),
                //    MonitorItemSchedule = new MonitorItemSchedule()
                //}
            });
            list.Add(new MonitorItemType()
            {
                Name = "Registry",
                Description = "Checks for particular registry setting(s)",
                ItemType = MonitorItemTypes.Registry,
                CheckerType = CheckerTypes.Registry,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType= SystemValueTypes.MIP_RegistryKey,
                        Value = "Key"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_RegistryValue,
                        Value= "Value"
                    }
                }
                //CreateMonitorItem = () => new MonitorRegistry() { ID = Guid.NewGuid().ToString(), 
                //            Name = "Registry [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "REST API",
                Description = "Checks REST API returns expected response",
                ItemType = MonitorItemTypes.REST,
                CheckerType = CheckerTypes.REST,
                DefaultParameters= new List<MonitorItemParameter>()
                { 
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_RESTURL,
                        Value = "http://myrestapi.com/test"
                    }
                }
                //CreateMonitorItem = () => new MonitorREST() { ID = Guid.NewGuid().ToString(), 
                //            Name = "REST API [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Run Process",
                Description = "Runs process and checks the exit code",
                ItemType = MonitorItemTypes.RunProcess,
                CheckerType = CheckerTypes.RunProcess,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType= SystemValueTypes.MIP_RunProcessFileName,
                        Value = "File.exe"
                    }
                }
                //CreateMonitorItem = () => new MonitorRunProcess() { ID = Guid.NewGuid().ToString(), 
                //            Name = "Run Process [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Service",
                Description = "Checks Windows service status",
                ItemType = MonitorItemTypes.Service,
                CheckerType = CheckerTypes.Service,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_ServiceMachineName,
                        Value = "Machine"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_ServiceServiceName,
                        Value = "My Service"
                    }
                }
                //CreateMonitorItem = () => new MonitorService() { ID = Guid.NewGuid().ToString(), 
                //            Name = "Service [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "SMTP",
                Description = "Checks SMTP connection",
                ItemType = MonitorItemTypes.SMTP,
                CheckerType = CheckerTypes.SMTP,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_SMTPPort,
                        Value = "100"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_SMTPServer,
                        Value = "Server"
                    }
                }
                //CreateMonitorItem = () => new MonitorSMTP() { ID = Guid.NewGuid().ToString(), 
                //            Name = "SMTP [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "SOAP",
                Description = "Checks SOAP API returns expected response",
                ItemType = MonitorItemTypes.SOAP,
                CheckerType = CheckerTypes.SOAP,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_SOAPURL,
                        Value = "http://soapurl/test"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_SOAPXML,
                        Value = "test"
                    }
                }
                //CreateMonitorItem = () => new MonitorSOAP() { ID = Guid.NewGuid().ToString(), 
                //            Name = "SOAP [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "TCP or UDP socket",
                Description = "Checks TCP or UDP socket",
                ItemType = MonitorItemTypes.Socket,
                CheckerType = CheckerTypes.Socket,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_SocketHost,
                        Value = "Host"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_SocketPort,
                        Value = "1000"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_SocketProtocol,
                        Value = "TCP"
                    }
                }
                //CreateMonitorItem = () => new MonitorSocket() { ID = Guid.NewGuid().ToString(), 
                //            Name = "TCP or UDP socket [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "SQL query",
                Description = "Checks results of SQL query",
                ItemType = MonitorItemTypes.SQL,
                CheckerType = CheckerTypes.SQL,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_SQLConnectionString,
                        Value= "Connection String"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType  = SystemValueTypes.MIP_SQLQuery,
                        Value = "select * from Test"
                    }
                }
                //CreateMonitorItem = () => new MonitorSQL() { ID = Guid.NewGuid().ToString(), 
                //            Name = "SQL Query [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "HTTP/HTTPS endpoint",
                Description = "Checks HTTP/HTTPS endpoint returns expected response",
                ItemType = MonitorItemTypes.URL,
                CheckerType = CheckerTypes.URL,
                DefaultParameters = new List<MonitorItemParameter>()
                {
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_URLMethod,
                        Value = "GET"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_URLPassword,
                        Value = "password"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType= SystemValueTypes.MIP_URLProxyName,
                        Value = "Proxy"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_URLProxyPort,
                        Value = "1000"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_URLURL,
                        Value = "http://google.co.uk/test"
                    },
                    new MonitorItemParameter()
                    {
                        SystemValueType = SystemValueTypes.MIP_URLUsername,
                        Value = "username"
                    }
                }
                //CreateMonitorItem = () => new MonitorURL() { ID = Guid.NewGuid().ToString(), 
                //            Name = "HTTP/HTTPS endpoint [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            return list;
        }
    }
}
