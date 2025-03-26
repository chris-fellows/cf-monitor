using CFMonitor.Interfaces;
using CFMonitor.Models;

namespace CFMonitor.Services
{
    public class EventItemFactoryService : IEventItemFactoryService
    {
        private readonly IMonitorItemTypeService _monitorItemTypeService;
        private readonly ISystemValueTypeService _systemValueTypeService;

        public EventItemFactoryService(IMonitorItemTypeService monitorItemTypeService, 
                                    ISystemValueTypeService systemValueTypeService)
        {
            _monitorItemTypeService = monitorItemTypeService;
            _systemValueTypeService = systemValueTypeService;
        }

        public List<EventItem> GetDefaultForMonitorItem(MonitorItem monitorItem)
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            // Get monitor item type, gives us list of event condition value types
            var monitorItemType = _monitorItemTypeService.GetAll().First(mit => mit.ItemType == monitorItem.MonitorItemType);

            // Add event item for each event condition value type
            var eventItems = new List<EventItem>();            
            foreach(var eventConditionValueType in monitorItemType.EventConditionValueTypes)
            {
                // Get system value type
                var systemValueType = systemValueTypes.First(svt => svt.ValueType == eventConditionValueType);

                // Create event item
                eventItems.Add(CreateEventItem(monitorItem, systemValueType));
            }

            return eventItems;
        }

        private static EventItem CreateEventItem(MonitorItem monitorItem, SystemValueType eventConditionSystemValueType)
        {
            var eventItem = new EventItem()
            {
                Id = Guid.NewGuid().ToString(),
                MonitorItemId = monitorItem.Id,
                ActionItems = new(),
                EventCondition = new()
                {
                    SourceValueType = eventConditionSystemValueType.DefaultEventCondition.SourceValueType,
                    Operator = eventConditionSystemValueType.DefaultEventCondition.Operator,
                    ValueTypeName = eventConditionSystemValueType.DefaultEventCondition.ValueTypeName,
                    Values = eventConditionSystemValueType.DefaultEventCondition.Values
                }
            };            

            return eventItem;            
        }
    }
}
