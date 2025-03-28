using CFMonitor.Constants;

namespace CFMonitor.Models.Messages
{
    public class MonitorAgentLogMessage : MessageBase
    {
        public string FileName { get; set; } = String.Empty;
        public byte[] Content { get; set; } = new byte[0];

        public MonitorAgentLogMessage()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.MonitorAgentLogMessage;
        }
    }
}
