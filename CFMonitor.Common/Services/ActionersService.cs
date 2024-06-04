using CFMonitor.Actioners;
using CFMonitor.Interfaces;
using System.Collections.Generic;

namespace CFMonitor.Services
{
    public class ActionersService : IActionersService
    {
        public List<IActioner> GetAll()
        {
            List<IActioner> actionerList = new List<IActioner>();
            actionerList.Add(new ActionerDatadogWarning());
            actionerList.Add(new ActionerEmail());
            actionerList.Add(new ActionerLog());
            actionerList.Add(new ActionerProcess());
            actionerList.Add(new ActionerSMS());
            actionerList.Add(new ActionerSQL());
            actionerList.Add(new ActionerURL());
            return actionerList;
        }
    }
}
