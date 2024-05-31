using CFMonitor.Enums;
using System;
using System.Collections.Generic;

namespace CFMonitor.Models
{
    public class EventConditionValuesService
    {
        public List<EventConditionItem> GetItems(EventConditionSource eventConditionSource)
        {
            switch (eventConditionSource)
            {
                case EventConditionSource.DriveAvailableFreeSpace:

                    break;
                case EventConditionSource.Exception:
                    break;
                case EventConditionSource.FileExists:
                    break;
                case EventConditionSource.FileNotExists:
                    break;
                case EventConditionSource.HostEntryExists:
                    break;
                case EventConditionSource.HostEntryNotExists:
                    break;
                case EventConditionSource.HttpResponseStatusCode:
                    break;
            }

            return null;
        }
    }
}
