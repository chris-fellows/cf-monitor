using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFWebServer.Services;

namespace CFMonitor.Services
{
    public class XmlAuditEventService : XmlEntityWithIdService<AuditEvent, string>, IAuditEventService
    {
        public XmlAuditEventService(string folder) : base(folder,
                                                "AuditEvent.*.xml",
                                              (auditEvent) => $"AuditEvent.{auditEvent.Id}.xml",
                                                (auditEventId) => $"AuditEvent.{auditEventId}.xml")
        {

        }

        public List<AuditEvent> GetByFilter(AuditEventFilter filter)
        {
            var auditEvents = GetAll()
                        .Where(i =>
                        (
                           (
                               filter.CreatedDateTimeFrom == null ||
                               i.CreatedDateTime >= filter.CreatedDateTimeFrom
                           ) &&
                           (
                               filter.CreatedDateTimeTo == null ||
                               i.CreatedDateTime <= filter.CreatedDateTimeTo
                           ) &&
                           (
                               filter.AuditEventTypeIds == null ||
                               !filter.AuditEventTypeIds.Any() ||
                               filter.AuditEventTypeIds.Contains(i.TypeId)
                           )                           
                        )).ToList();

            return auditEvents;
        }
    }
}
