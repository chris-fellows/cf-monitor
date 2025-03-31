
using CFMonitor.Constants;

namespace CFMonitor.Models.Messages
{
    /// <summary>
    /// Monitor item has been updated
    /// </summary>
    public class EntityUpdated : MessageBase
    {
        public string EntityId { get; set; } = String.Empty;

        public string SystemValueTypeId { get; set; } = String.Empty;

        public EntityUpdated()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.EntityUpdated;
        }
    }
}
