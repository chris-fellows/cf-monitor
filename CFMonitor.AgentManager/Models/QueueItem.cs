using CFConnectionMessaging.Models;

namespace CFMonitor.AgentManager.Models
{
    public class QueueItem
    {
        public ConnectionMessage? ConnectionMessage { get; set; }

        public MessageReceivedInfo? MessageReceivedInfo { get; set; }
    }
}
