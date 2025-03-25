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
                    DataType = SystemValueDataTypes.String,                 
                },
                   new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Error message",
                    ValueType = SystemValueTypes.AEP_ErrorMessage,
                    DataType = SystemValueDataTypes.String,
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Monitor Agent Id",
                    ValueType = SystemValueTypes.AEP_MonitorAgentId,
                    DataType = SystemValueDataTypes.String,
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Monitor Item Id",
                    ValueType = SystemValueTypes.AEP_MonitorItemId,
                    DataType = SystemValueDataTypes.String,
                },
                 new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Monitor Item Output Id",
                    ValueType = SystemValueTypes.AEP_MonitorItemOutputId,
                    DataType = SystemValueDataTypes.String,
                },
                    new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Notes",
                    ValueType = SystemValueTypes.AEP_Notes,
                    DataType = SystemValueDataTypes.String,
                },
                     new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Password Reset Id",
                    ValueType = SystemValueTypes.AEP_PasswordResetId,
                    DataType = SystemValueDataTypes.String,
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "User Id",
                    ValueType = SystemValueTypes.AEP_UserId,
                    DataType = SystemValueDataTypes.String,
                },

                // -----------------------------------------------------------------------------------------------------------------
                // Event condition sources
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Active processing running",
                    ValueType = SystemValueTypes.ECS_ActiveProcessRunning,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_ActiveProcessRunning,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "CPU in tolerance",
                    ValueType= SystemValueTypes.ECS_CPUInTolerance,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_CPUInTolerance,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    // TODO: Check this
                    Id= Guid.NewGuid().ToString(),
                    Name = "Disk space available bytes",
                    ValueType = SystemValueTypes.ECS_DiskSpaceAvailableBytes,
                    DataType = SystemValueDataTypes.Integer,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_DiskSpaceAvailableBytes,
                        Operator = ConditionOperators.LessThan,
                        Values = new List<object>() { 1000000 }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "DNS host exists",
                    ValueType = SystemValueTypes.ECS_DNSHostExists,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_DNSHostExists,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Exception",
                    ValueType = SystemValueTypes.ECS_Exception,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_Exception,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { true }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "File size in tolerance",
                    ValueType = SystemValueTypes.ECS_FileSizeInTolerance,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_FileSizeInTolerance,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Folder size in tolerance",
                    ValueType = SystemValueTypes.ECS_FolderSizeInTolerance,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_FolderSizeInTolerance,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "HTTP response status code",
                    ValueType = SystemValueTypes.ECS_HTTPResponseStatusCode,
                    DataType = SystemValueDataTypes.HttpStatusCode,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_HTTPResponseStatusCode,
                        Operator = ConditionOperators.NotEquals,
                        Values = new List<object>() { System.Net.HttpStatusCode.OK }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "IMAP connected",
                    ValueType= SystemValueTypes.ECS_IMAPConnected,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_IMAPConnected,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Local file exists",
                    ValueType = SystemValueTypes.ECS_LocalFileExists,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_LocalFileExists,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Local file text found",
                    ValueType = SystemValueTypes.ECS_LocalFileTextFound,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_LocalFileTextFound,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Memory in tolerance",
                    ValueType = SystemValueTypes.ECS_MemoryInTolerance,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_MemoryInTolerance,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "NTP time in tolerance",
                    ValueType = SystemValueTypes.ECS_NTPTimeInTolerance,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_NTPTimeInTolerance,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "",
                    ValueType  = SystemValueTypes.ECS_PingReplyStatus,
                    DataType = SystemValueDataTypes.PingReplyStatus,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_PingReplyStatus,
                        Operator = ConditionOperators.NotEquals,
                        Values = new List<object>() { IPStatus.Success }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "POP connected",
                    ValueType = SystemValueTypes.ECS_POPConnected,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_POPConnected,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Run process exist code",
                    ValueType = SystemValueTypes.ECS_RunProcessExitCode,
                    DataType = SystemValueDataTypes.Integer,
                    MinValue = Int32.MinValue.ToString(),
                    MaxValue = Int32.MaxValue.ToString(),
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_RunProcessExitCode,
                        Operator = ConditionOperators.NotEquals,
                        Values = new List<object>() { 0 }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Socket connected",
                    ValueType = SystemValueTypes.ECS_SocketConnected,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_SocketConnected,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { false }
                    }
                },
                new SystemValueType()
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "SQL returns rows",
                    ValueType = SystemValueTypes.ECS_SQLReturnsRows,
                    DataType = SystemValueDataTypes.Boolean,
                    DefaultEventCondition = new EventCondition()
                    {
                        SourceValueType  = SystemValueTypes.ECS_SQLReturnsRows,
                        Operator = ConditionOperators.Equals,
                        Values = new List<object>() { true }
                    }
                },                

                // -----------------------------------------------------------------------------------------------------------------
                // Monitor item parameters
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Active process file name",
                    ValueType = SystemValueTypes.MIP_ActiveProcessFileName,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Active process machine name",
                    ValueType = SystemValueTypes.MIP_ActiveProcessMachineName,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "CPU server",
                    ValueType = SystemValueTypes.MIP_CPUServer,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "DHCP server",
                    ValueType = SystemValueTypes.MIP_DHCPServer,
                    DataType = SystemValueDataTypes.String
                },

                 new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Drive",
                    ValueType = SystemValueTypes.MIP_DiskSpaceDrive,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Host",
                    ValueType = SystemValueTypes.MIP_DNSHost,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "File",
                    ValueType = SystemValueTypes.MIP_FileSizeFile,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Max file size (bytes)",
                    ValueType = SystemValueTypes.MIP_FileSizeMaxFileSizeBytes,
                    DataType = SystemValueDataTypes.Integer,
                    MinValue = "0",
                    MaxValue = Int64.MaxValue.ToString()
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Folder",
                    ValueType = SystemValueTypes.MIP_FolderSizeFolder,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Max folder size (bytes)",
                    ValueType = SystemValueTypes.MIP_FolderSizeMaxFolderSizeBytes,
                    DataType = SystemValueDataTypes.Integer,
                    MinValue = "0",
                    MaxValue = Int64.MaxValue.ToString()
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "File name",
                    ValueType = SystemValueTypes.MIP_LocalFileFileName,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Find text",
                    ValueType = SystemValueTypes.MIP_LocalFileFindText,
                    DataType = SystemValueDataTypes.String
                },                               
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Server",
                    ValueType = SystemValueTypes.MIP_PingServer,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "URL",
                    ValueType = SystemValueTypes.MIP_RESTURL,
                    DataType = SystemValueDataTypes.Url
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Process file name",
                    ValueType = SystemValueTypes.MIP_RunProcessFileName,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SMTP port",
                    ValueType = SystemValueTypes.MIP_SMTPPort,
                    DataType = SystemValueDataTypes.Integer,
                    MinValue = "1",
                    MaxValue = Int32.MaxValue.ToString()
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SMTP server",
                    ValueType = SystemValueTypes.MIP_SMTPServer,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "URL",
                    ValueType = SystemValueTypes.MIP_SOAPURL,
                    DataType = SystemValueDataTypes.Url
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "XML",
                    ValueType = SystemValueTypes.MIP_SOAPXML,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Host",
                    ValueType = SystemValueTypes.MIP_SocketHost,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Port",
                    ValueType = SystemValueTypes.MIP_SocketPort,
                    DataType = SystemValueDataTypes.Integer,
                    MinValue = "1",
                    MaxValue = Int32.MaxValue.ToString()
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Protocol (TCP/UDP)",
                    ValueType = SystemValueTypes.MIP_SocketProtocol,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Connection string",
                    ValueType = SystemValueTypes.MIP_SQLConnectionString,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SQL",
                    ValueType = SystemValueTypes.MIP_SQLSQL,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Max time tolerance (Secs)",
                    ValueType = SystemValueTypes.MIP_TimeMaxToleranceSecs,
                    DataType = SystemValueDataTypes.Integer
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Time server",
                    ValueType = SystemValueTypes.MIP_TimeServer,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Time server type",
                    ValueType = SystemValueTypes.MIP_TimeServerType,
                    DataType = SystemValueDataTypes.TimeServerType
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Method (GET/PUT/POST/DELETE)",
                    ValueType = SystemValueTypes.MIP_URLMethod,
                    DataType = SystemValueDataTypes.HttpMethod
                    //AllowedValues = SystemValueDataTypeUtilities.GetAllowedValues(SystemValueDataTypes.HttpMethod)                    
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Password",
                    ValueType = SystemValueTypes.MIP_URLPassword,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Proxy name",
                    ValueType = SystemValueTypes.MIP_URLProxyName,
                    DataType = SystemValueDataTypes.String
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Proxy port",
                    ValueType = SystemValueTypes.MIP_URLProxyPort,
                    DataType = SystemValueDataTypes.Integer,
                    MinValue = "1",
                    MaxValue = Int32.MaxValue.ToString()
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "URL",
                    ValueType = SystemValueTypes.MIP_URLURL,
                    DataType = SystemValueDataTypes.Url
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Username",
                    ValueType = SystemValueTypes.MIP_URLUsername,
                    DataType = SystemValueDataTypes.String
                }
            };

            return list;
        }
    }
}
