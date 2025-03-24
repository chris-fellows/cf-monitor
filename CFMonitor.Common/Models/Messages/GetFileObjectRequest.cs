using CFMonitor.Constants;

namespace CFMonitor.Models.Messages
{
    public class GetFileObjectRequest : MessageBase
    {
        public string FileObjectId { get; set; } = String.Empty;

        public GetFileObjectRequest()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.GetFileObjectRequest;
        }
    }
}
