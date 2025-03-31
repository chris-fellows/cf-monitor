using CFConnectionMessaging.Models;
using CFMonitor.AgentManager.Enums;

namespace CFMonitor.AgentManager.Models
{
    public class QueueItem
    {
        public QueueItemTypes ItemType { get; set; }

        public ConnectionMessage? ConnectionMessage { get; set; }

        public MessageReceivedInfo? MessageReceivedInfo { get; set; }
    }
}
