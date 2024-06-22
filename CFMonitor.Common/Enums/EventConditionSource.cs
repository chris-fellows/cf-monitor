namespace CFMonitor.Enums
{
    /// <summary>
    /// What to check when determining if an event condition is true
    /// </summary>
    public enum EventConditionSource
    {
        Exception = 0,
        NoException = 1,
        DriveAvailableFreeSpace = 2,
        HostEntryExists = 3,
        HostEntryNotExists = 4,
        FileExists = 5,
        FileNotExists = 6,
        TextFoundInFile = 7,
        TextNotFoundInFile = 8,
        ActiveProcessRunning = 9,
        ActiveProcessNotRunning = 10,
        HttpResponseStatusCode = 11,
        SocketConnected = 12,
        SocketNotConnected = 13,
        ServiceControllerStatus = 14,
        PingReplyStatus = 15,
        SQLReturnsRows = 16,
        SQLReturnsNoRows = 17,
        WebExceptionStatus = 18,
        RunProcessExitCodeReturned = 19,
        NTPTimeInTolerance = 20,
        NTPTimeOutsideTolerance = 21,
        CPUInTolerance = 22,
        CPUOutsideTolerance = 23,
        FolderSizeInTolerance = 24,
        FolderSizeOutsideTolerance = 25,
        FileSizeInTolerance = 26,
        FileSizeOutsideTolerance = 27,
        MemoryInTolerance = 28,
        MemoryOutsideTolerance = 29,
        POPConnected = 30,
        POPConnectError = 31,
        IMAPConnected = 32,
        IMAPConnectError = 33
    }
}
