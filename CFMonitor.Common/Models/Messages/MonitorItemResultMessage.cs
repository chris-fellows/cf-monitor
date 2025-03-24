using CFMonitor.Constants;

namespace CFMonitor.Models.Messages
{
    /// <summary>
    /// Result of checking monitor item
    /// </summary>
    public class MonitorItemResultMessage : MessageBase
    {
        public MonitorItemOutput MonitorItemOutput { get; set; }

        public MonitorItemResultMessage()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.MonitorItemResultMessage;
        }
    }
}
