using CFMonitor.EntityReader;
using CFMonitor.Constants;
using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Utilities;
using System.Net.NetworkInformation;

namespace CFMonitor.Seed
{
    public class SystemValueTypeSeed1 : IEntityReader<SystemValueType>
    {
        public IEnumerable<SystemValueType> Read()
        {
            var list = new List<SystemValueType>()
            {
                // -----------------------------------------------------------------------------------------------------------------
                // Audit event parameters
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Action Item Id",
                    ValueType = SystemValueTypes.AEP_ActionItemId,
                    ValueTypeName = typeof(String).FullName                    
                },
                   new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Error message",
                    ValueType = SystemValueTypes.AEP_ErrorMessage,
                    ValueTypeName = typeof(String).FullName,                  
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Monitor Agent Id",
                    ValueType = SystemValueTypes.AEP_MonitorAgentId,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Monitor Item Id",
                    ValueType = SystemValueTypes.AEP_MonitorItemId,
                    ValueTypeName = typeof(String).FullName,                    
                },
                 new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Monitor Item Output Id",
                    ValueType = SystemValueTypes.AEP_MonitorItemOutputId,
                    ValueTypeName = typeof(String).FullName,                    
                },
                    new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Notes",
                    ValueType = SystemValueTypes.AEP_Notes,
                    ValueTypeName = typeof(String).FullName,                    
                },
                     new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Password Reset Id",
                    ValueType = SystemValueTypes.AEP_PasswordResetId,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "User Id",
                    ValueType = SystemValueTypes.AEP_UserId,
                    ValueTypeName = typeof(String).FullName,                    
                },

                // -----------------------------------------------------------------------------------------------------------------
                // Event condition sources
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Active processing running",
                    ValueType = SystemValueTypes.ECS_ActiveProcessRunning,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_ActiveProcessRunning,
                        Operator = ConditionOperators.Equals,
                        ValueTypeName = typeof(Boolean).FullName,
                        Values = new List<string>() { "false" }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "CPU in tolerance",
                    ValueType= SystemValueTypes.ECS_CPUInTolerance,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_CPUInTolerance,
                        Operator = ConditionOperators.Equals,
                        ValueTypeName = typeof(Boolean).FullName,
                        Values = new List<string>() { "false" }
                    }
                },
                new SystemValueType()
                {
                    // TODO: Check this
                    Id= Guid.NewGuid().ToString(),
                    Name = "Disk space available bytes",
                    ValueType = SystemValueTypes.ECS_DiskSpaceAvailableBytes,
                    ValueTypeName = typeof(Int32).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_DiskSpaceAvailableBytes,
                        Operator = ConditionOperators.LessThan,
                        ValueTypeName = typeof(Int32).FullName,
                        Values = new List<string>() { "1000000" }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "DNS host exists",
                    ValueType = SystemValueTypes.ECS_DNSHostExists,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_DNSHostExists,
                        Operator = ConditionOperators.Equals,
                         ValueTypeName = typeof(Boolean).FullName,
                          Values = new List<string>() { "false" }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Exception",
                    ValueType = SystemValueTypes.ECS_Exception,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_Exception,
                        Operator = ConditionOperators.Equals,
                         ValueTypeName = typeof(Boolean).FullName,
                           Values = new List<string>() { "true" }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "File size in tolerance",
                    ValueType = SystemValueTypes.ECS_FileSizeInTolerance,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_FileSizeInTolerance,
                        Operator = ConditionOperators.Equals,
                         ValueTypeName = typeof(Boolean).FullName,
                         Values = new List<string>() { "false" }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Folder size in tolerance",
                    ValueType = SystemValueTypes.ECS_FolderSizeInTolerance,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_FolderSizeInTolerance,
                        Operator = ConditionOperators.Equals,
                         ValueTypeName = typeof(Boolean).FullName,
                            Values = new List<string>() { "false" }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "HTTP response status code",
                    ValueType = SystemValueTypes.ECS_HTTPResponseStatusCode,
                    ValueTypeName = typeof(System.Net.HttpStatusCode).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_HTTPResponseStatusCode,
                        Operator = ConditionOperators.NotEquals,
                         ValueTypeName = typeof(System.Net.HttpStatusCode).FullName,
                        Values = new List<string>() { System.Net.HttpStatusCode.OK.ToString() }
                        //Values = new List<object>() { System.Net.HttpStatusCode.OK }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "IMAP connected",
                    ValueType= SystemValueTypes.ECS_IMAPConnected,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_IMAPConnected,
                        Operator = ConditionOperators.Equals,
                        ValueTypeName = typeof(Boolean).FullName,
                        Values = new List<string>() { "false" }
                        //Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Local file exists",
                    ValueType = SystemValueTypes.ECS_LocalFileExists,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_LocalFileExists,
                        Operator = ConditionOperators.Equals,
                         ValueTypeName = typeof(Boolean).FullName,
                        Values = new List<string>() { "false" }
                        //Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Local file text found",
                    ValueType = SystemValueTypes.ECS_LocalFileTextFound,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_LocalFileTextFound,
                        Operator = ConditionOperators.Equals,
                         ValueTypeName = typeof(Boolean).FullName,
                        Values = new List<string>() { "false" }
                        //Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Memory in tolerance",
                    ValueType = SystemValueTypes.ECS_MemoryInTolerance,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_MemoryInTolerance,
                        Operator = ConditionOperators.Equals,
                         ValueTypeName = typeof(Boolean).FullName,
                        Values = new List<string>() { "false" }
                        //Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "NTP time in tolerance",
                    ValueType = SystemValueTypes.ECS_NTPTimeInTolerance,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_NTPTimeInTolerance,
                        Operator = ConditionOperators.Equals,
                         ValueTypeName = typeof(Boolean).FullName,
                        Values = new List<string>() { "false" }
                        //Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "",
                    ValueType  = SystemValueTypes.ECS_PingReplyStatus,
                    ValueTypeName = typeof(IPStatus).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_PingReplyStatus,
                        Operator = ConditionOperators.NotEquals,
                        ValueTypeName = typeof(IPStatus).FullName,
                        Values = new List<string>() { IPStatus.Success.ToString() }
                        //Values = new List<object>() { IPStatus.Success }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "POP connected",
                    ValueType = SystemValueTypes.ECS_POPConnected,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_POPConnected,
                        Operator = ConditionOperators.Equals,
                        ValueTypeName = typeof(Boolean).FullName,
                        Values = new List<string>() { "false" }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Run process exist code",
                    ValueType = SystemValueTypes.ECS_RunProcessExitCode,
                    ValueTypeName = typeof(Int32).FullName,                    
                    MinValue = Int32.MinValue.ToString(),
                    MaxValue = Int32.MaxValue.ToString(),
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_RunProcessExitCode,
                        Operator = ConditionOperators.NotEquals,
                        ValueTypeName = typeof(Int32).FullName,
                        Values = new List<string>() { "0" }
                        //Values = new List<object>() { 0 }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Socket connected",
                    ValueType = SystemValueTypes.ECS_SocketConnected,
                    ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_SocketConnected,
                        Operator = ConditionOperators.Equals,
                         ValueTypeName = typeof(Boolean).FullName,
                        Values = new List<string>() { "false" }
                        //Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "SQL returns rows",
                    ValueType = SystemValueTypes.ECS_SQLReturnsRows,
                       ValueTypeName = typeof(Boolean).FullName,                    
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_SQLReturnsRows,
                        Operator = ConditionOperators.Equals,
                         ValueTypeName = typeof(Boolean).FullName,
                        Values = new List<string>() { "true" }
                        //Values = new List<object>() { true }
                    }
                },                

                // -----------------------------------------------------------------------------------------------------------------
                // Monitor item parameters
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Active process file name",
                    ValueType = SystemValueTypes.MIP_ActiveProcessFileName,
                       ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Active process machine name",
                    ValueType = SystemValueTypes.MIP_ActiveProcessMachineName,
                       ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "CPU server",
                    ValueType = SystemValueTypes.MIP_CPUServer,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "DHCP server",
                    ValueType = SystemValueTypes.MIP_DHCPServer,
                    ValueTypeName = typeof(String).FullName,                    
                },

                 new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Drive",
                    ValueType = SystemValueTypes.MIP_DiskSpaceDrive,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Host",
                    ValueType = SystemValueTypes.MIP_DNSHost,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "File",
                    ValueType = SystemValueTypes.MIP_FileSizeFile,
                    ValueTypeName = typeof(String).FullName,                  
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Max file size (bytes)",
                    ValueType = SystemValueTypes.MIP_FileSizeMaxFileSizeBytes,
                    ValueTypeName = typeof(Int32).FullName,                    
                    MinValue = "0",
                    MaxValue = Int64.MaxValue.ToString()
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Folder",
                    ValueType = SystemValueTypes.MIP_FolderSizeFolder,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Max folder size (bytes)",
                    ValueType = SystemValueTypes.MIP_FolderSizeMaxFolderSizeBytes,
                    ValueTypeName = typeof(Int32).FullName,                    
                    MinValue = "0",
                    MaxValue = Int64.MaxValue.ToString()
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "File name",
                    ValueType = SystemValueTypes.MIP_LocalFileFileName,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Find text",
                    ValueType = SystemValueTypes.MIP_LocalFileFindText,
                    ValueTypeName = typeof(String).FullName,                    
                },                               
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Server",
                    ValueType = SystemValueTypes.MIP_PingServer,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "URL",
                    ValueType = SystemValueTypes.MIP_RESTURL,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Process file name",
                    ValueType = SystemValueTypes.MIP_RunProcessFileName,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Process file object Id",
                    ValueType = SystemValueTypes.MIP_RunProcessFileObjectId,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SMTP port",
                    ValueType = SystemValueTypes.MIP_SMTPPort,
                    ValueTypeName = typeof(Int32).FullName,                    
                    MinValue = "1",
                    MaxValue = Int32.MaxValue.ToString()
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SMTP server",
                    ValueType = SystemValueTypes.MIP_SMTPServer,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "URL",
                    ValueType = SystemValueTypes.MIP_SOAPURL,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "XML",
                    ValueType = SystemValueTypes.MIP_SOAPXML,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Host",
                    ValueType = SystemValueTypes.MIP_SocketHost,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Port",
                    ValueType = SystemValueTypes.MIP_SocketPort,
                    ValueTypeName = typeof(Int32).FullName,                    
                    MinValue = "1",
                    MaxValue = Int32.MaxValue.ToString()
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Protocol (TCP/UDP)",
                    ValueType = SystemValueTypes.MIP_SocketProtocol,
                    ValueTypeName = typeof(String).FullName,
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SQL connection string",
                    ValueType = SystemValueTypes.MIP_SQLConnectionString,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SQL query",
                    ValueType = SystemValueTypes.MIP_SQLSQL,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SQL file object Id",
                    ValueType = SystemValueTypes.MIP_SQLSQLFileObjectId,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Max time tolerance (Secs)",
                    ValueType = SystemValueTypes.MIP_TimeMaxToleranceSecs,
                    ValueTypeName = typeof(Int32).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Time server",
                    ValueType = SystemValueTypes.MIP_TimeServer,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Time server type",
                    ValueType = SystemValueTypes.MIP_TimeServerType,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Method (GET/PUT/POST/DELETE)",
                    ValueType = SystemValueTypes.MIP_URLMethod,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Password",
                    ValueType = SystemValueTypes.MIP_URLPassword,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Proxy name",
                    ValueType = SystemValueTypes.MIP_URLProxyName,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Proxy port",
                    ValueType = SystemValueTypes.MIP_URLProxyPort,
                    ValueTypeName = typeof(Int32).FullName,                    
                    MinValue = "1",
                    MaxValue = Int32.MaxValue.ToString()
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "URL",
                    ValueType = SystemValueTypes.MIP_URLURL,
                    ValueTypeName = typeof(String).FullName,                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Username",
                    ValueType = SystemValueTypes.MIP_URLUsername,
                    ValueTypeName = typeof(String).FullName,                    
                }
            };

            return list;
        }
    }
}
