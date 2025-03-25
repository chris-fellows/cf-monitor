using CFMonitor.Constants;

namespace CFMonitor.Models.Messages
{
    public class GetEventItemsResponse : MessageBase
    {
        public List<EventItem> EventItems { get; set; } = new();

        public GetEventItemsResponse()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.GetEventItemsResponse;
        }
    }
}
