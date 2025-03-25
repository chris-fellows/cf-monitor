using CFMonitor.Constants;

namespace CFMonitor.Models.Messages
{
    public class GetSystemValueTypesResponse : MessageBase
    {
        public List<SystemValueType> SystemValueTypes { get; set; } = new();

        public GetSystemValueTypesResponse()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.GetSystemValueTypesResponse;
        }
    }
}
