using CFMonitor.Models.Messages;
using CFMonitor.SystemTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Agent.SystemTask
{
    public class HeartbeatTask : ISystemTask
    {
        public string Name => "Heartbeat";
        
        public Task ExecuteAsync(Dictionary<string, object> parameters, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            //var managerConnection = (ManagerConnection)parameters["ManagerConnection"];

            //var heartbeat = new Heartbeat() { Id = Guid.NewGuid().ToString() };
            //managerConnection.SendHeartbeat(heartbeat, null);

            return Task.CompletedTask;
        }
    }
}
