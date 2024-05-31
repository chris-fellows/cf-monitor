namespace CFMonitor.Enums
{
    public enum ScheduleTypes : byte
    {
        FixedInterval = 0,       // E.g. Every N secs
        FixedTimes = 1           // E.g. At 12:00 & 18:00 every Mon, Wed & Fri
    }
}
