namespace CFMonitor.Constants
{
    public static class MessageTypeIds
    {
        public const string GetFileObjectRequest = "GetFileObjectRequest";
        public const string GetFileObjectResponse = "GetFileObjectResponse";

        public const string GetEventItemsRequest = "GetEventItemsRequest";
        public const string GetEventItemsResponse = "GetEventItemsResponse";

        public const string GetMonitorAgentsRequest = "GetMonitorAgentsRequest";
        public const string GetMonitorAgentsResponse = "GetMonitorAgentsResponse";

        public const string GetMonitorItemsRequest = "GetMonitorItemsRequest";
        public const string GetMonitorItemsResponse = "GetMonitorItemsResponse";
        
        public const string GetSystemValueTypesRequest = "GetSystemValueTypesRequest";
        public const string GetSystemValueTypesResponse = "GetSystemValueTypesResponse";
        
        public const string Heartbeat = "Heartbeat";
        public const string MonitorAgentLogMessage = "MonitorAgentLogMessage";

        public const string MonitorItemResultMessage = "MonitorItemResultMessage";
        public const string EntityUpdated = "EntityUpdated";        
    }
}
