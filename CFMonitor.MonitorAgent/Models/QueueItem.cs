using CFConnectionMessaging.Models;
using CFMonitor.Agent.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Agent.Models
{
    public class QueueItem
    {
        public QueueItemTypes ItemType { get; set; }

        public ConnectionMessage? ConnectionMessage { get; set; }

        public MessageReceivedInfo? MessageReceivedInfo { get; set; }
    }
}
