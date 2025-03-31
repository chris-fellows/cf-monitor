using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using CFMonitor.Constants;
using CFMonitor.Models.Messages;

namespace CFMonitor.MessageConverters
{
    public class EntityUpdatedConverter : IExternalMessageConverter<EntityUpdated>
    {
        public ConnectionMessage GetConnectionMessage(EntityUpdated entityUpdated)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = entityUpdated.Id,
                TypeId = MessageTypeIds.EntityUpdated,
                Parameters = new List<ConnectionMessageParameter>()
                {
                    new ConnectionMessageParameter()
                    {
                        Name = "EntityId",
                        Value= entityUpdated.EntityId
                    },
                    new ConnectionMessageParameter()
                    {
                        Name = "SystemValueTypeId",
                        Value= entityUpdated.SystemValueTypeId
                    }
                }
            };
            return connectionMessage;
        }

        public EntityUpdated GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var entityUpdated = new EntityUpdated()
            {
                Id = connectionMessage.Id,
                EntityId = connectionMessage.Parameters.First(p => p.Name == "EntityId").Value,
                SystemValueTypeId = connectionMessage.Parameters.First(p => p.Name == "SystemValueTypeId").Value,
            };

            return entityUpdated;
        }
    }
}
