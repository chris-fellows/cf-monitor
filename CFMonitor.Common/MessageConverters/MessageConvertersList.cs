using CFConnectionMessaging.Interfaces;
using CFMonitor.Common.MessageConverters;
using CFMonitor.Models.Messages;

namespace CFMonitor.MessageConverters
{
    /// <summary>
    /// Message converters for converting between ConnectionMessage and external message
    /// </summary>
    public class MessageConvertersList
    {          
        private readonly IExternalMessageConverter<GetEventItemsRequest> _getEventItemsRequestConverter = new GetEventItemsRequestConverter();
        private readonly IExternalMessageConverter<GetEventItemsResponse> _getEventItemsResponseConverter = new GetEventItemsResponseConverter();

        private readonly IExternalMessageConverter<GetFileObjectRequest> _getFileObjectRequestConverter = new GetFileObjectRequestConverter();
        private readonly IExternalMessageConverter<GetFileObjectResponse> _getFileObjectResponseConverter = new GetFileObjectResponseConverter();

        private readonly IExternalMessageConverter<GetMonitorAgentsRequest> _getMonitorAgentsRequestConverter = new GetMonitorAgentsRequestConverter();
        private readonly IExternalMessageConverter<GetMonitorAgentsResponse> _getMonitorAgentsResponseConverter = new GetMonitorAgentsResponseConverter();

        private readonly IExternalMessageConverter<GetMonitorItemsRequest> _getMonitorItemsRequestConverter = new GetMonitorItemsRequestConverter();
        private readonly IExternalMessageConverter<GetMonitorItemsResponse> _getMonitorItemsResponseConverter = new GetMonitorItemsResponseConverter();

        private readonly IExternalMessageConverter<GetSystemValueTypesRequest> _getSystemValueTypesRequestConverter = new GetSystemValueTypesRequestConverter();
        private readonly IExternalMessageConverter<GetSystemValueTypesResponse> _getSystemValueTypesResponseConverter = new GetSystemValueTypesResponseConverter();

        private readonly IExternalMessageConverter<Heartbeat> _heartbeatConverter = new HeartbeatConverter();

        private readonly IExternalMessageConverter<MonitorItemResultMessage> _monitorItemResultMessageConverter = new MonitorItemResultMessageConverter();
        private readonly IExternalMessageConverter<MonitorItemUpdated> _monitorItemUpdatedConverter = new MonitorItemUpdatedConverter();


        public IExternalMessageConverter<GetEventItemsRequest> GetEventItemsRequestConverter => _getEventItemsRequestConverter;
        public IExternalMessageConverter<GetEventItemsResponse> GetEventItemsResponseConverter => _getEventItemsResponseConverter;

        public IExternalMessageConverter<GetFileObjectRequest> GetFileObjectRequestConverter => _getFileObjectRequestConverter;
        public IExternalMessageConverter<GetFileObjectResponse> _GetFileObjectResponseConverter => _getFileObjectResponseConverter;

        public IExternalMessageConverter<GetMonitorAgentsRequest> GetMonitorAgentsRequestConverter => _getMonitorAgentsRequestConverter;
        public IExternalMessageConverter<GetMonitorAgentsResponse> GetMonitorAgentsResponseConverter => _getMonitorAgentsResponseConverter;

        public IExternalMessageConverter<GetMonitorItemsRequest> GetMonitorItemsRequestConverter => _getMonitorItemsRequestConverter;
        public IExternalMessageConverter<GetMonitorItemsResponse> GetMonitorItemsResponseConverter => _getMonitorItemsResponseConverter;

        public IExternalMessageConverter<GetSystemValueTypesRequest> GetSystemValueTypesRequestConverter => _getSystemValueTypesRequestConverter;
        public IExternalMessageConverter<GetSystemValueTypesResponse> GetSystemValueTypesResponseConverter => _getSystemValueTypesResponseConverter;

        public IExternalMessageConverter<Heartbeat> HeartbeatConverter => _heartbeatConverter;

        public IExternalMessageConverter<MonitorItemResultMessage> MonitorItemResultMessageConverter => _monitorItemResultMessageConverter;
        public IExternalMessageConverter<MonitorItemUpdated> MonitorItemUpdatedConverter => _monitorItemUpdatedConverter;
    }
}
