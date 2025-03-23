using CFMonitor.EntityReader;
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
        private readonly ISystemValueTypeService _systemValueTypeService;

        public EventItemSeed1(IEventItemFactoryService eventItemFactoryService,
                    IMonitorItemService monitorItemService,
                    IMonitorItemTypeService monitorItemTypeService,
                    ISystemValueTypeService systemValueTypeService)
        {
            _eventItemFactoryService = eventItemFactoryService; 
            _monitorItemService = monitorItemService;
            _monitorItemTypeService = monitorItemTypeService;
            _systemValueTypeService = systemValueTypeService;
        }

        public IEnumerable<EventItem> Read()
        {
            var monitorItems = _monitorItemService.GetAll();
            var monitorItemTypes = _monitorItemTypeService.GetAll();
            var systemValueTypes = _systemValueTypeService.GetAll();

            var list = new List<EventItem>();
            list.AddRange(CreateEventsSQL(monitorItems.First(mi => mi.Name == "Monitor SQL"), monitorItemTypes, systemValueTypes));
            list.AddRange(CreateEventsActiveProcess(monitorItems.First(mi => mi.Name == "Check Process"), monitorItemTypes, systemValueTypes));
            list.AddRange(CreateEventsLocalFile(monitorItems.First(mi => mi.Name == "Check Log File Exists"), monitorItemTypes, systemValueTypes));
            list.AddRange(CreateEventsPing(monitorItems.First(mi => mi.Name == "Ping Google"), monitorItemTypes, systemValueTypes));
            list.AddRange(CreateEventsURL(monitorItems.First(mi => mi.Name == "Check Google Website"), monitorItemTypes, systemValueTypes));
            list.AddRange(CreateEventsFolderSize(monitorItems.First(mi => mi.Name == "Check Temp Folder Size"), monitorItemTypes, systemValueTypes));
            list.AddRange(CreateEventsCPU(monitorItems.First(mi => mi.Name == "Check CPU"), monitorItemTypes, systemValueTypes));
            list.AddRange(CreateEventsMemory(monitorItems.First(mi => mi.Name == "Check Memory"), monitorItemTypes, systemValueTypes));
            list.AddRange(CreateEventsNTP(monitorItems.First(mi => mi.Name == "Check NTP Time"), monitorItemTypes, systemValueTypes));

            return list;
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

        private List<EventItem> CreateEventsSQL(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes, List<SystemValueType> systemValueTypes)
        {
         

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);
            
            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_SQLReturnsRows);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Query returned data", "The query returned data"));

            return new() { eventItem };            
        }

        private List<EventItem> CreateEventsActiveProcess(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes, List<SystemValueType> systemValueTypes)
        {
       
            var svtActiveProcessFileName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_ActiveProcessFileName);

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_ActiveProcessRunning);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Process not running",
                        string.Format("The process {0} is not running", monitorItem.Parameters.First(p => p.SystemValueTypeId == svtActiveProcessFileName.Id).Value)));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsLocalFile(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes, List<SystemValueType> systemValueTypes)
        {
        
            var svtLocalFileName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_LocalFileFileName);

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_LocalFileExists);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("File not found",
                    string.Format("The file {0} was not found", monitorItem.Parameters.First(p => p.SystemValueTypeId == svtLocalFileName.Id).Value)));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsPing(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes, List<SystemValueType> systemValueTypes)
        {
        
            var svtServer = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_PingServer);

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_PingReplyStatus);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Ping failed",
                        string.Format("Ping {0} failed", monitorItem.Parameters.First(p => p.SystemValueTypeId == svtServer.Id).Value)));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsURL(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes, List<SystemValueType> systemValueTypes)
        {
            var monitorItemType = monitorItemTypes.First(mit => mit.ItemType == monitorItem.MonitorItemType);

         
            var svtUrl = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLURL);

            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_HTTPResponseStatusCode);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Web connection failed",
                        string.Format("Error opening URL {0}", monitorItem.Parameters.First(p => p.SystemValueTypeId == svtUrl.Id).Value)));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsFolderSize(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes, List<SystemValueType> systemValueTypes)
        {
           
            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_FolderSizeInTolerance);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Folder size above threshold",
                            "The folder size is above the threshold"));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsFileSize(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes, List<SystemValueType> systemValueTypes)
        {
         
            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_FileSizeInTolerance);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("File size above threshold",
                            "The file size is above the threshold"));

            return new() { eventItem };
        }

        private List<EventItem> CreateEventsCPU(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes, List<SystemValueType> systemValueTypes)
        {
          
            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_CPUInTolerance);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("CPU above threshold",
                            "The CPU is above the threshold"));

            return new() { eventItem };     
        }

        private List<EventItem> CreateEventsMemory(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes, List<SystemValueType> systemValueTypes)
        {           
            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_MemoryInTolerance);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Memory use above threshold",
                            "The memory use is above the threshold"));

            return new() { eventItem };
        }

        private  List<EventItem> CreateEventsNTP(MonitorItem monitorItem, List<MonitorItemType> monitorItemTypes, List<SystemValueType> systemValueTypes)
        {        
            var eventItems = _eventItemFactoryService.GetDefaultForMonitorItem(monitorItem);

            var eventItem = eventItems.First(ei => ei.EventCondition.SourceValueType == SystemValueTypes.ECS_NTPTimeInTolerance);
            eventItem.ActionItems.Add(CreateDefaultActionEmail("Machine time incorrect",
                            "The machine time is different from the NTP server and outside the threshold"));

            return new() { eventItem };
        }
    }
}
