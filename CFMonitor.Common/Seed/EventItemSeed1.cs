using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class EventItemSeed1 : IEntityReader<EventItem>
    {
        private readonly IEventItemFactoryService _eventItemFactoryService;
        private readonly IMonitorItemService _monitorItemService;
        private readonly IMonitorItemTypeService _monitorItemTypeService;

        public EventItemSeed1(IEventItemFactoryService eventItemFactoryService,
                    IMonitorItemService monitorItemService,
                    IMonitorItemTypeService monitorItemTypeService)
        {
            _eventItemFactoryService = eventItemFactoryService;
            _monitorItemService = monitorItemService;
            _monitorItemTypeService = monitorItemTypeService;
        }

        public Task<List<EventItem>> ReadAllAsync()
        {
            var monitorItems = _monitorItemService.GetAll();
            var monitorItemTypes = _monitorItemTypeService.GetAll();

            var list = new List<EventItem>();
            list.AddRange(CreateEventsSQL(monitorItems.First(mi => mi.Name == "Monitor SQL"), monitorItemTypes));
            list.AddRange(CreateEventsActiveProcess(monitorItems.First(mi => mi.Name == "Check Process"), monitorItemTypes));
            list.AddRange(CreateEventsLocalFile(monitorItems.First(mi => mi.Name == "Check Log File Exists"), monitorItemTypes));
            list.AddRange(CreateEventsPing(monitorItems.First(mi => mi.Name == "Ping Google"), monitorItemTypes));
            list.AddRange(CreateEventsURL(monitorItems.First(mi => mi.Name == "Check Google Website"), monitorItemTypes));
            list.AddRange(CreateEventsFolderSize(monitorItems.First(mi => mi.Name == "Check Temp Folder Size"), monitorItemTypes));
            list.AddRange(CreateEventsCPU(monitorItems.First(mi => mi.Name == "Check CPU"), monitorItemTypes));
            list.AddRange(CreateEventsMemory(monitorItems.First(mi => mi.Name == "Check Memory"), monitorItemTypes));
            list.AddRange(CreateEventsNTP(monitorItems.First(mi => mi.Name == "Check NTP Time"), monitorItemTypes));

            return Task.FromResult(list);
        }

        private static ActionItem CreateDefaultActionEmail(string subject, string body)
        {
            ActionItem action = new ActionItem()
            {
                //Subject = subject,
                //Body = body
            };
            //action.RecipientList.Add(MonitorItemTestFactory.DeveloperEmail);
            //action.Server = "MyEmailServer";
            return action;
        }

        private static ActionItem CreateDefaultActionLog(string body)
        {
            ActionItem action = new ActionItem()
            {
                //LogFileName = "D:\\Temp\\Logs\\MyLog.txt"
            };
            return action;
        }

        private static ActionItem CreateDefaultActionConsole(string body)
        {
            ActionItem action = new ActionItem()
            {

            };
            return action;
        }

        private List<EventItem> CreateEventsSQL(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes)
        {
            /*
            var monitorItemType = monitorItemTypes.First(mit => mit.ItemType == monitorItem.MonitorItemType);            

            // Add event for Status not success
            EventItem eventItem = new EventItem();
            eventItem.Id = Guid.NewGuid().ToString();
            eventItem.MonitorItemId = monitorItem.Id;
            eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_SQLReturnsRows;
            eventItem.EventCondition.Operator = ConditionOperators.Equals;
            eventItem.EventCondition.Values.Add(true);
            */

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_SQLReturnsRows);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Query returned data", "The query returned data"));
           
            return new() { eventItem };
        }

        private List<EventItem> CreateEventsActiveProcess(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes)
        {
            //var monitorItemType = monitorItemTypes.First(mit => mit.ItemType == monitorItem.MonitorItemType);

            //EventItem eventItem = new EventItem();
            //eventItem.Id = Guid.NewGuid().ToString();
            //eventItem.MonitorItemId = monitorItem.Id;
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_ActiveProcessRunning;
            //eventItem.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem.EventCondition.Values.Add(false);

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_ActiveProcessRunning);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Process not running",
                        string.Format("The process {0} is not running", monitorItem.Parameters.First(p => p.SystemValueTypeId == svtActiveProcessFileName.Id).Value)));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsLocalFile(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes)
        {
            //var monitorItemType = monitorItemTypes.First(mit => mit.ItemType == monitorItem.MonitorItemType);

            //EventItem eventItem = new EventItem();
            //eventItem.Id = Guid.NewGuid().ToString();
            //eventItem.MonitorItemId = monitorItem.Id;
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_LocalFileExists;
            //eventItem.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem.EventCondition.Values.Add(false);

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_LocalFileExists);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("File not found",
                    string.Format("The file {0} was not found", monitorItem.Parameters.First(p => p.SystemValueTypeId == svtLocalFileName.Id).Value)));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsPing(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes)
        {
            //var monitorItemType = monitorItemTypes.First(mit => mit.ItemType == monitorItem.MonitorItemType);

            //// Add event for Status not success
            //EventItem eventItem = new EventItem();
            //eventItem.Id = Guid.NewGuid().ToString();
            //eventItem.MonitorItemId = monitorItem.Id;
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_PingReplyStatus;
            //eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
            //eventItem.EventCondition.Values.Add(System.Net.NetworkInformation.IPStatus.Success);

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_PingReplyStatus);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Ping failed",
                        string.Format("Ping {0} failed", monitorItem.Parameters.First(p => p.SystemValueTypeId == svtServer.Id).Value)));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsURL(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes)
        {
            var monitorItemType = monitorItemTypes.First(mit => mit.ItemType == monitorItem.MonitorItemType);

            //// Add event for StatusCode not OK, send email
            //EventItem eventItem = new EventItem();
            //eventItem.Id = Guid.NewGuid().ToString();
            //eventItem.MonitorItemId = monitorItem.Id;
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_HTTPResponseStatusCode;
            //eventItem.EventCondition.Operator = ConditionOperators.NotEquals;
            //eventItem.EventCondition.Values.Add(System.Net.HttpStatusCode.OK);            

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_HTTPResponseStatusCode);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Web connection failed",
                        string.Format("Error opening URL {0}", monitorItem.Parameters.First(p => p.SystemValueTypeId == svtUrl.Id).Value)));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsFolderSize(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes)
        {
            //var monitorItemType = monitorItemTypes.First(mit => mit.ItemType == monitorItem.MonitorItemType);

            //EventItem eventItem = new EventItem();
            //eventItem.Id = Guid.NewGuid().ToString();
            //eventItem.MonitorItemId = monitorItem.Id;
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_FolderSizeInTolerance;
            //eventItem.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem.EventCondition.Values.Add(false);

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_FolderSizeInTolerance);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Folder size above threshold",
                            "The folder size is above the threshold"));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsFileSize(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes)
        {
            //var monitorItemType = monitorItemTypes.First(mit => mit.ItemType == monitorItem.MonitorItemType);

            //EventItem eventItem = new EventItem();
            //eventItem.Id = Guid.NewGuid().ToString();
            //eventItem.MonitorItemId = monitorItem.Id;
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_FileSizeInTolerance;
            //eventItem.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem.EventCondition.Values.Add(false);

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_FileSizeInTolerance);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("File size above threshold",
                            "The file size is above the threshold"));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsCPU(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes)
        {
            //var monitorItemType = monitorItemTypes.First(mit => mit.ItemType == monitorItem.MonitorItemType);

            //EventItem eventItem = new EventItem();
            //eventItem.Id = Guid.NewGuid().ToString();
            //eventItem.MonitorItemId = monitorItem.Id;
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_CPUInTolerance;
            //eventItem.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem.EventCondition.Values.Add(false);

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_CPUInTolerance);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("CPU above threshold",
                            "The CPU is above the threshold"));

            return new() { eventItem };     
        }

        private List<EventItem> CreateEventsMemory(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes)
        {
            //var monitorItemType = monitorItemTypes.First(mit => mit.ItemType == monitorItem.MonitorItemType);

            //EventItem eventItem = new EventItem();
            //eventItem.Id = Guid.NewGuid().ToString();
            //eventItem.MonitorItemId = monitorItem.Id;
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_MemoryInTolerance;
            //eventItem.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem.EventCondition.Values.Add(false);

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_MemoryInTolerance);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Memory use above threshold",
                            "The memory use is above the threshold"));

            return new() { eventItem };
        }

        private  List<EventItem> CreateEventsNTP(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes)
        {
            //var monitorItemType = monitorItemTypes.First(mit => mit.ItemType == monitorItem.MonitorItemType);

            //EventItem eventItem = new EventItem();
            //eventItem.Id = Guid.NewGuid().ToString();
            //eventItem.MonitorItemId = monitorItem.Id;
            //eventItem.EventCondition.SourceValueType = SystemValueTypes.ECS_NTPTimeInTolerance;
            //eventItem.EventCondition.Operator = ConditionOperators.Equals;
            //eventItem.EventCondition.Values.Add(false);            

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_NTPTimeInTolerance);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Machine time incorrect",
                            "The machine time is different from the NTP server and outside the threshold"));

            return new() { eventItem };
        }
    }
}
