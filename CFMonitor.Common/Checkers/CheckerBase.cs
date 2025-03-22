using CFMonitor.Interfaces;
using CFMonitor.Models;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Base checker
    /// </summary>
    public abstract class CheckerBase
    {
        protected readonly IEventItemService _eventItemService;
        protected readonly ISystemValueTypeService _systemValueTypeService;

        public CheckerBase(IEventItemService eventItemService, 
                        ISystemValueTypeService systemValueTypeService)
        {
            _eventItemService = eventItemService;
            _systemValueTypeService = systemValueTypeService;
        }

        /// <summary>
        /// Executes action
        /// </summary>
        /// <param name="actionerList"></param>
        /// <param name="monitorItem"></param>
        /// <param name="actionItem"></param>
        /// <param name="actionItemParameters"></param>
        protected async Task ExecuteActionAsync(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> actionItemParameters)
        {
            foreach (IActioner actioner in actionerList)
            {
                if (actioner.CanExecute(actionItem))
                {
                    await actioner.ExecuteAsync(monitorItem, actionItem, actionItemParameters);
                    break;
                }
            }
        }
    }
}
