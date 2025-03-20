using CFMonitor.Enums;
using System;
using System.Collections.Generic;

namespace CFMonitor.Models
{
    public class EventConditionValuesService
    {
        public List<EventConditionItem> GetItems(EventConditionSources eventConditionSource)
        {
            switch (eventConditionSource)
            {
                case EventConditionSources.DriveAvailableFreeSpace:

                    break;
                case EventConditionSources.Exception:
                    break;
                case EventConditionSources.FileExists:
                    break;
                case EventConditionSources.FileNotExists:
                    break;
                case EventConditionSources.HostEntryExists:
                    break;
                case EventConditionSources.HostEntryNotExists:
                    break;
                case EventConditionSources.HttpResponseStatusCode:
                    break;
            }

            return null;
        }
    }
}
