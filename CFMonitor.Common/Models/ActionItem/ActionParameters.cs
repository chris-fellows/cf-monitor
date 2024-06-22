using CFMonitor.Common.Enums;
using CFMonitor.Enums;
using System.Collections.Generic;

namespace CFMonitor.Models.ActionItems
{
    public class ActionParameters
    {
        public Dictionary<ActionParameterTypes, object> Values = new Dictionary<ActionParameterTypes, object>();
    }
}
