namespace CFMonitor.Enums
{
    public enum  ActionerTypes : byte
    {
        Console,
        DatadogWarning,
        Email,
        EventLog,
        Log,
        Process,
        MachineRestart,
        //ServiceRestart,
        SMS,
        SQL,
        URL
    }
}
