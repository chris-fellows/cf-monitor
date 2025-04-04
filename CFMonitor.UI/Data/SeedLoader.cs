﻿using CFMonitor.Enums;
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
            var contentTemplateService = scope.ServiceProvider.GetRequiredService<IContentTemplateService>();
            var eventItemService = scope.ServiceProvider.GetRequiredService<IEventItemService>();
            var fileObjectService = scope.ServiceProvider.GetRequiredService<IFileObjectService>();
            var monitorAgentService = scope.ServiceProvider.GetRequiredService<IMonitorAgentService>();
            var monitorAgentGroupService = scope.ServiceProvider.GetRequiredService<IMonitorAgentGroupService>();
            var monitorAgentManagerService = scope.ServiceProvider.GetRequiredService<IMonitorAgentManagerService>();
            var monitorItemService = scope.ServiceProvider.GetRequiredService<IMonitorItemService>();
            var notificationGroupService = scope.ServiceProvider.GetRequiredService<INotificationGroupService>();
            var systemTaskStatusService = scope.ServiceProvider.GetRequiredService<ISystemTaskStatusService>();
            var systemTaskTypeService = scope.ServiceProvider.GetRequiredService<ISystemTaskTypeService>();
            var systemValueTypeService = scope.ServiceProvider.GetRequiredService<ISystemValueTypeService>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            // Get seed serviced
            var actionItemTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<ActionItemType>>("ActionItemTypeSeed");
            var auditEventTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<AuditEventType>>("AuditEventTypeSeed");
            var contentTemplateSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<ContentTemplate>>("ContentTemplateSeed");
            var eventItemSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<EventItem>>("EventItemSeed");
            var fileObjectSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<FileObject>>("FileObjectSeed");
            var monitorAgentGroupSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<MonitorAgentGroup>>("MonitorAgentGroupSeed");
            var monitorAgentManagerSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<MonitorAgentManager>>("MonitorAgentManagerSeed");
            var monitorAgentSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<MonitorAgent>>("MonitorAgentSeed");
            var monitorItemSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<MonitorItem>>("MonitorItemSeed");            
            var notificationGroupSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<NotificationGroup>>("NotificationGroupSeed");
            var systemTaskStatusSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<SystemTaskStatus>>("SystemTaskStatusSeed");
            var systemTaskTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<SystemTaskType>>("SystemTaskTypeSeed");
            var systemValueTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<SystemValueType>>("SystemValueTypeSeed");
            var userSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<User>>("UserSeed");

            // Add system value types
            var systemValueTypesNew = systemValueTypeSeed.Read();
            foreach(var systemValueType in systemValueTypesNew)
            {
                await systemValueTypeService.AddAsync(systemValueType);
            }

            // Add action item types
            var actionItemTypesNew = actionItemTypeSeed.Read();
            foreach(var actionItemType in actionItemTypesNew)
            {
                await actionItemTypeService.AddAsync(actionItemType);
            }

            // Add notification groups
            var notificationGroupsNews = notificationGroupSeed.Read();
            foreach (var notificationGroup in notificationGroupsNews)
            {
               await notificationGroupService.AddAsync(notificationGroup);
            }

            // Add audit event types
            var auditEventTypesNew = auditEventTypeSeed.Read();
            foreach (var auditEventType in auditEventTypesNew)
            {
                await auditEventTypeService.AddAsync(auditEventType);
            }

            // Add system statuses
            var systemTaskStatusesNew = systemTaskStatusSeed.Read();
            foreach (var systemTaskStatus in systemTaskStatusesNew)
            {
                await systemTaskStatusService.AddAsync(systemTaskStatus);
            }

            // Add system task types
            var systemTaskTypesNew = systemTaskTypeSeed.Read();
            foreach(var systemTaskType in systemTaskTypesNew)
            {
                await systemTaskTypeService.AddAsync(systemTaskType);
            }

            // Add content templates
            var contentTemplatesNew = contentTemplateSeed.Read();
            foreach (var contentTemplate in contentTemplatesNew)
            {
                await contentTemplateService.AddAsync(contentTemplate);
            }         

            // Add file objects
            var fileObjectsNew = fileObjectSeed.Read();
            foreach (var fileObject in fileObjectsNew)
            {
                await fileObjectService.AddAsync(fileObject);
            }

            // Add users
            // Need to add "User added" audit event afterwards because we need the system user
            var usersNew = userSeed.Read();
            foreach(var user in usersNew)
            {
                await userService.AddAsync(user);                
            }

            // Add "User added" audit event
            var users = userService.GetAll();
            var systemUser = users.First(u => u.GetUserType() == Enums.UserTypes.System);
            foreach(var user in users)
            {
                await auditEventService.AddAsync(auditEventFactory.CreateUserAdded(systemUser.Id, user.Id));
            }

            // Add monitor agent managers
            var monitorAgentManagersNew = monitorAgentManagerSeed.Read();
            foreach(var monitorAgentManager in monitorAgentManagersNew)
            {
                await monitorAgentManagerService.AddAsync(monitorAgentManager);
            }

            // Add monitor agent groups
            var monitorAgentGroupsNew = monitorAgentGroupSeed.Read();
            foreach (var monitorAgentGroup in monitorAgentGroupsNew)
            {
                await monitorAgentGroupService.AddAsync(monitorAgentGroup);
            }

            // Add monitor agents. Depends on monitor agent groups
            var monitorAgentsNew =  monitorAgentSeed.Read();
            foreach (var monitorAgent in monitorAgentsNew)
            {
                await monitorAgentService.AddAsync(monitorAgent);

                // Add audit event
                await auditEventService.AddAsync(auditEventFactory.CreateMonitorAgentAdded(systemUser.Id, monitorAgent.Id));
            }

            // Add monitor items            
            var monitorItemsNew = monitorItemSeed.Read();
            foreach(var monitorItem in monitorItemsNew)
            {                
                await monitorItemService.AddAsync(monitorItem);

                // Add audit event
                await auditEventService.AddAsync(auditEventFactory.CreateMonitorItemAdded(systemUser.Id, monitorItem.Id, usersNew.ToList()[0].Id));
            }

            // Add event items. Depends on monitor items
            var eventItemsNew = eventItemSeed.Read();
            foreach(var eventItem in eventItemsNew)
            {
                await eventItemService.AddAsync(eventItem);
            }
        }
    }
}
