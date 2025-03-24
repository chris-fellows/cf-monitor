using CFMonitor.Constants;

namespace CFMonitor.Models.Messages
{
    /// <summary>
    /// Heartbeat from Monitor Agent
    /// </summary>
    public class Heartbeat : MessageBase
    {       
        public string MachineName { get; set; } = String.Empty;

        public string UserName { get; set; } = String.Empty;

        public Heartbeat()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.Heartbeat;                
        }
    }
}
