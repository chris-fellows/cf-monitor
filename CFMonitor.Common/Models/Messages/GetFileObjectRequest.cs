namespace CFMonitor.Models.Messages
{
    public class GetFileObjectRequest : MessageBase
    {
        public string FileObjectId { get; set; } = String.Empty;
    }
}
