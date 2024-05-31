using CFMonitor.Enums;
using System;
using System.Collections.Generic;

namespace CFMonitor
{
    public class EventConditionItem
    {
        public ConditionOperators Operator { get; set; }

        public List<object> Values { get; set; }
    }
}
