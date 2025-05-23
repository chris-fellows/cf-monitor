﻿using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFWebServer.Services;

namespace CFMonitor.Services
{
    public class XmlAuditEventTypeService : XmlEntityWithIdAndNameService<AuditEventType, string>, IAuditEventTypeService
    {
        public XmlAuditEventTypeService(string folder) : base(folder,
                                                "AuditEventType.*.xml",
                                              (auditEventType) => $"AuditEventType.{auditEventType.Id}.xml",
                                                (auditEventTypeId) => $"AuditEventType.{auditEventTypeId}.xml",
                                                (auditEventType) => auditEventType.Name)
        {

        }
    }
}
