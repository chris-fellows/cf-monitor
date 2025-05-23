﻿namespace CFMonitor.Enums
{
    /// <summary>
    /// System value types   
    /// 
    /// Split in to categories:
    /// - AEP_ : Audit event parameter types
    /// - ECS_ : Event condition sources
    /// - AIP_ : Action item parameters
    /// - MIP_ : Monitor item parameters
    /// </summary>
    public enum SystemValueTypes
    {
        // -----------------------------------------------------------------------------------------------------
        // Audit event parameter types        
        AEP_ActionItemId,
        AEP_ContentTemplateId,
        AEP_ErrorMessage,
        AEP_FileObjectId,
        AEP_MonitorAgentId,
        AEP_MonitorAgentGroupId,
        AEP_MonitorAgentManagerId,
        AEP_MonitorItemId,
        AEP_MonitorItemOutputId,
        AEP_Notes,
        AEP_PasswordResetId,
        AEP_UserId,

        // -----------------------------------------------------------------------------------------------------
        // Event condition sources
        ECS_Exception,

        ECS_DiskSpaceAvailableBytes,

        ECS_DNSHostExists,

        ECS_LocalFileExists,
        ECS_LocalFileTextFound,

        ECS_ActiveProcessRunning,

        ECS_HTTPResponseStatusCode,

        ECS_SocketConnected,

        ECS_PingReplyStatus,        // Values=List of statuses

        ECS_SQLReturnsRows,

        ECS_RunProcessExitCode,

        ECS_NTPTimeInTolerance,

        ECS_CPUInTolerance,

        ECS_FileSizeInTolerance,
        
        ECS_FolderSizeInTolerance,

        ECS_MemoryInTolerance,

        ECS_IMAPConnected,

        ECS_POPConnected,

        // -----------------------------------------------------------------------------------------------------
        // Action item parameters
        AIP_EmailBody,
        AIP_EmailCC,
        AIP_EmailRecipient,
        AIP_EmailSender,
        AIP_EmailServer,
        AIP_EmailSubject,
        AIP_EmailUsername,

        AIP_EventLogEventId,
        AIP_EventLogLogName,

        AIP_LogLogFileName,

        AIP_ProcessFileName,

        //AIP_ServiceRestartServiceName,

        AIP_SMSNumber,

        AIP_SQLConnectionString,
        AIP_SQLSQL,

        AIP_URLHeader,
        AIP_URLURL,
        AIP_URLMethod,
        AIP_URLPassword,
        AIP_URLProxyName,
        AIP_URLProxyPort,
        AIP_URLUsername,

        // -----------------------------------------------------------------------------------------------------
        // Action item parameters (Custom - Generated during the check)
        AIPC_ErrorMessage,        
        AIPC_Message,

        // -----------------------------------------------------------------------------------------------------
        // Monitor item parameters
        MIP_ActiveProcessFileName,
        MIP_ActiveProcessMachineName,

        MIP_CPUServer,

        MIP_DHCPServer,

        MIP_DiskSpaceDrive,

        MIP_DNSHost,

        MIP_FileSizeFile,
        MIP_FileSizeMaxFileSizeBytes,

        MIP_FolderSizeFolder,
        MIP_FolderSizeMaxFolderSizeBytes,

        MIP_LocalFileFileName,
        MIP_LocalFileFindText,
        
        MIP_TimeServerType,
        MIP_TimeServer,
        MIP_TimeMaxToleranceSecs,

        MIP_PingServer,

        //MIP_RegistryKey,
        //MIP_RegistryValue,

        MIP_RESTURL,

        MIP_RunProcessFileName,
        MIP_RunProcessFileObjectId,

        //MIP_ServiceMachineName,
        //MIP_ServiceServiceName,

        MIP_SMTPServer,
        MIP_SMTPPort,

        MIP_SOAPURL,
        MIP_SOAPXML,

        MIP_SocketHost,
        MIP_SocketPort,
        MIP_SocketProtocol,

        MIP_SQLConnectionString,
        MIP_SQLSQL,
        MIP_SQLSQLFileObjectId,

        MIP_URLURL,
        MIP_URLMethod,
        MIP_URLPassword,
        MIP_URLProxyName,
        MIP_URLProxyPort,
        MIP_URLUsername,        
    }
}
