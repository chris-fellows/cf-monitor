namespace CFMonitor.Enums
{
    /// <summary>
    /// System value data types. It is typically used by the UI for controlling what value can be entered
    /// for a system value when configuring monitor item parameters.
    /// 
    /// Default is String which allows any format data.
    /// </summary>
    public enum SystemValueDataTypes
    {
        Boolean,
        Decimal,
        String,     // Default
        HttpMethod,
        HttpStatusCode,
        PingReplyStatus,
        Integer,
        Url
    }
}
