
using CFMonitor.Constants;

namespace CFMonitor.Models.Messages
{
    /// <summary>
    /// Monitor item has been updated
    /// </summary>
    public class MonitorItemUpdated : MessageBase
    {
        public MonitorItemUpdated()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.MonitorItemUpdated;
        }
    }
}
