using CFMonitor.Enums;
using CFMonitor.EntityReader;
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
            var actionItemTypeService = scope.ServiceProvider.GetRequiredService<IActionItemTypeService>();
            var auditEventFactory = scope.ServiceProvider.GetRequiredService<IAuditEventFactory>();
            var auditEventService = scope.ServiceProvider.GetRequiredService<IAuditEventService>();
            var auditEventTypeService = scope.ServiceProvider.GetRequiredService<IAuditEventTypeService>();
            var eventItemService = scope.ServiceProvider.GetRequiredService<IEventItemService>();
            var fileObjectService = scope.ServiceProvider.GetRequiredService<IFileObjectService>();
            var monitorAgentService = scope.ServiceProvider.GetRequiredService<IMonitorAgentService>();
            var monitorAgentGroupService = scope.ServiceProvider.GetRequiredService<IMonitorAgentGroupService>();
            var monitorItemService = scope.ServiceProvider.GetRequiredService<IMonitorItemService>();
            var systemValueTypeService = scope.ServiceProvider.GetRequiredService<ISystemValueTypeService>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            // Get seed serviced
            var actionItemTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<ActionItemType>>("ActionItemTypeSeed");
            var auditEventTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<AuditEventType>>("AuditEventTypeSeed");
            var eventItemSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<EventItem>>("EventItemSeed");
            var fileObjectSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<FileObject>>("FileObjectSeed");
            var monitorAgentGroupSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<MonitorAgentGroup>>("MonitorAgentGroupSeed");
            var monitorAgentSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<MonitorAgent>>("MonitorAgentSeed");
            var monitorItemSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<MonitorItem>>("MonitorItemSeed");
            var systemValueTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<SystemValueType>>("SystemValueTypeSeed");
            var userSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<User>>("UserSeed");

            // Add system value types
            var systemValueTypesNew = systemValueTypeSeed.Read();
            foreach(var systemValueType in systemValueTypesNew)
            {
                systemValueTypeService.Add(systemValueType);
            }

            // Add action item types
            var actionItemTypesNew = actionItemTypeSeed.Read();
            foreach(var actionItemType in actionItemTypesNew)
            {
                actionItemTypeService.Add(actionItemType);
            }

            // Add audit event types
            var auditEventTypesNew = auditEventTypeSeed.Read();
            foreach (var auditEventType in auditEventTypesNew)
            {
                auditEventTypeService.Add(auditEventType);
            }

            // Add file objects
            var fileObjectsNew = fileObjectSeed.Read();
            foreach (var fileObject in fileObjectsNew)
            {
                fileObjectService.Add(fileObject);
            }

            // Add users
            // Need to add "User added" audit event afterwards because we need the system user
            var usersNew = userSeed.Read();
            foreach(var user in usersNew)
            {
                userService.Add(user);                
            }

            // Add "User added" audit event
            var users = userService.GetAll();
            var systemUser = users.First(u => u.GetUserType() == Enums.UserTypes.System);
            foreach(var user in users)
            {
                auditEventService.Add(auditEventFactory.CreateUserAdded(systemUser.Id, user.Id));
            }

            // Add monitor agent groups
            var monitorAgentGroupsNew = monitorAgentGroupSeed.Read();
            foreach (var monitorAgentGroup in monitorAgentGroupsNew)
            {
                monitorAgentGroupService.Add(monitorAgentGroup);
            }

            // Add monitor agents. Depends on monitor agent groups
            var monitorAgentsNew =  monitorAgentSeed.Read();
            foreach (var monitorAgent in monitorAgentsNew)
            {
                monitorAgentService.Add(monitorAgent);

                // Add audit event
                auditEventService.Add(auditEventFactory.CreateMonitorAgentAdded(systemUser.Id, monitorAgent.Id));
            }

            // Add monitor items            
            var monitorItemsNew = monitorItemSeed.Read();
            foreach(var monitorItem in monitorItemsNew)
            {                
                monitorItemService.Add(monitorItem);

                // Add audit event
                auditEventService.Add(auditEventFactory.CreateMonitorItemAdded(systemUser.Id, monitorItem.Id, usersNew.ToList()[0].Id));
            }

            // Add event items. Depends on monitor items
            var eventItemsNew = eventItemSeed.Read();
            foreach(var eventItem in eventItemsNew)
            {
                eventItemService.Add(eventItem);
            }
        }
    }
}
