using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
using CFUtilities.Models;
using CFUtilities.Services;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Base checker
    /// </summary>
    public abstract class CheckerBase
    {
        protected readonly IAuditEventFactory _auditEventFactory;
        protected readonly IAuditEventService _auditEventService;
        protected readonly IAuditEventTypeService _auditEventTypeService;
        protected readonly IEventItemService _eventItemService;
        protected readonly IPlaceholderService _placeholderService;
        protected readonly ISystemValueTypeService _systemValueTypeService;

        public CheckerBase(IAuditEventFactory auditEventFactory,
                        IAuditEventService auditEventService,
                        IAuditEventTypeService auditEventTypeService,
                        IEventItemService eventItemService, 
                        IPlaceholderService placeholderService,
                        ISystemValueTypeService systemValueTypeService)
        {
            _auditEventFactory = auditEventFactory;
            _auditEventService = auditEventService;
            _auditEventTypeService = auditEventTypeService;
            _eventItemService = eventItemService;
            _placeholderService = placeholderService;
            _systemValueTypeService = systemValueTypeService;
        }
        
        /// <summary>
        /// Sets placeholders for being replaced in parameter values.
        /// </summary>
        /// <param name="monitorAgent"></param>
        /// <param name="monitorItem"></param>
        /// <param name="checkerConfig"></param>
        protected void SetPlaceholders(MonitorAgent monitorAgent, MonitorItem monitorItem, CheckerConfig checkerConfig)
        {
            _placeholderService.ResetDefaults();

            // Add placeholder for root folder for files
            var placeholder1 = new Placeholder()
            {
                Name = _placeholderService.StartCharacters + "files-root-folder" + _placeholderService.EndCharacters,
                CanGetValue = (placeholderName) => placeholderName.StartsWith(_placeholderService.StartCharacters + "files-root-folder"),
                GetValue = (placeholderName, parameters) => checkerConfig.FilesRootFolder
            };
            _placeholderService.Add(placeholder1);

            //// Add placeholder for Monitor Agent Id
            //var placeholder2 = new Placeholder()
            //{
            //    Name = _placeholderService.StartCharacters + "monitor-agent-id" + _placeholderService.EndCharacters,
            //    CanGetValue = (placeholderName) => placeholderName.StartsWith(_placeholderService.StartCharacters + "monitor-agent-id"),
            //    GetValue = (placeholderName, parameters) => monitorAgent.Id
            //};
            //_placeholderService.Add(placeholder1);
        }

        /// <summary>
        /// Returns parameter value with placeholders replaced. E.g. Replaces {environment-variable:MY_PATH} with environment
        /// variable value
        /// </summary>
        /// <param name="monitorItemParameter"></param>
        /// <returns></returns>
        protected string? GetValueWithPlaceholdersReplaced(MonitorItemParameter? monitorItemParameter)                                        
        {
            return monitorItemParameter == null ? null :
                    _placeholderService.GetWithPlaceholdersReplaced(monitorItemParameter.Value, new());
        }

        ///// <summary>
        ///// Executes action
        ///// </summary>
        ///// <param name="actionerList"></param>
        ///// <param name="monitorItem"></param>
        ///// <param name="actionItem"></param>
        ///// <param name="actionItemParameters"></param>
        //protected async Task ExecuteActionAsync(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> actionItemParameters)
        //{
        //    foreach (IActioner actioner in actionerList)
        //    {
        //        if (actioner.CanExecute(actionItem))
        //        {
        //            await actioner.ExecuteAsync(monitorItem, actionItem, actionItemParameters);
        //            break;
        //        }
        //    }
        //}

        //protected void AddAuditEventMonitorItemChecking(MonitorAgent monitorAgent, MonitorItem monitorItem)
        //{
        //    var auditEvent = _auditEventFactory.CreateCheckingMonitorItem(monitorAgent.Id, monitorItem.Id);
        //    _auditEventService.Add(auditEvent);
        //}

        //protected void AddAuditEventMonitorItemChecked(MonitorAgent monitorAgent, MonitorItem monitorItem)
        //{
        //    var auditEvent = _auditEventFactory.CreateCheckedMonitorItem(monitorAgent.Id, monitorItem.Id);
        //    _auditEventService.Add(auditEvent);            
        //}
    }
}
