namespace CFMonitor.Enums
{
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
        ProcessRunning = 9,
        ProcessNotRunning = 10,
        HttpResponseStatusCode = 11,
        SocketConnected = 12,
        SocketNotConnected = 13,
        ServiceControllerStatus = 14,
        PingReplyStatus = 15,
        SQLReturnsRows = 16,
        SQLReturnsNoRows = 17,
        WebExceptionStatus = 18
    }
}
