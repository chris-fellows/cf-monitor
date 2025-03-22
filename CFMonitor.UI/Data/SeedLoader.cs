using CFMonitor.Interfaces;
using CFMonitor.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CFMonitor.UI.Data
{
    public class SeedLoader
    {
        public async Task LoadAsync(IServiceScope scope)
        {
            // Get data services
            var eventItemService = scope.ServiceProvider.GetRequiredService<IEventItemService>();
            var monitorAgentService = scope.ServiceProvider.GetRequiredService<IMonitorAgentService>();
            var monitorItemService = scope.ServiceProvider.GetRequiredService<IMonitorItemService>();
            var systemValueTypeService = scope.ServiceProvider.GetRequiredService<ISystemValueTypeService>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            // Get seed serviced
            var eventItemSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<EventItem>>("EventItemSeed");
            var monitorAgentSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<MonitorAgent>>("MonitorAgentSeed");
            var monitorItemSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<MonitorItem>>("MonitorItemSeed");
            var systemValueTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<SystemValueType>>("SystemValueTypeSeed");
            var userSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<User>>("UserSeed");

            // Add system value types
            var systemValueTypesNew = await systemValueTypeSeed.ReadAllAsync();
            foreach(var systemValueType in systemValueTypesNew)
            {
                systemValueTypeService.Add(systemValueType);
            }

            // Add users
            var usersNew = await userSeed.ReadAllAsync();
            foreach(var user in usersNew)
            {
                userService.Add(user);
            }

            // Add monitor agents
            var monitorAgentsNew = await monitorAgentSeed.ReadAllAsync();
            foreach (var monitorAgent in monitorAgentsNew)
            {
                monitorAgentService.Add(monitorAgent);
            }

            // Add monitor items
            var monitorItemsNew = await monitorItemSeed.ReadAllAsync();
            foreach(var monitorItem in monitorItemsNew)
            {
                monitorItemService.Add(monitorItem);
            }

            // Add event items. Depends on monitor items
            var eventItemsNew = await eventItemSeed.ReadAllAsync();
            foreach(var eventItem in eventItemsNew)
            {
                eventItemService.Add(eventItem);
            }
        }
    }
}
