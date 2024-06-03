using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;

namespace CFMonitor.Services
{
    public class MonitorItemTypeService : IMonitorItemTypeService
    {
        public List<MonitorItemType> GetAll()
        {
            var list = new List<MonitorItemType>();
            list.Add(new MonitorItemType()
            {
                Name = "DCHP",
                ItemType = MonitorItemTypes.DHCP,
                CheckerType = CheckerTypes.DHCP,
                CreateMonitorItem = () => new MonitorDHCP() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Disk space",
                ItemType = MonitorItemTypes.DiskSpace,
                CheckerType = CheckerTypes.DiskSpace,
                CreateMonitorItem = () => new MonitorDiskSpace() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "DNS",
                ItemType = MonitorItemTypes.DNS,
                CheckerType = CheckerTypes.DNS,
                CreateMonitorItem = () => new MonitorDNS() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "File",
                ItemType = MonitorItemTypes.LocalFile,
                CheckerType = CheckerTypes.LocalFile,
                CreateMonitorItem = () => new MonitorLocalFile() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "JSON",
                ItemType = MonitorItemTypes.JSON,
                CheckerType = CheckerTypes.JSON,
                CreateMonitorItem = () => new MonitorJSON() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "LDAP",
                ItemType = MonitorItemTypes.LDAP,
                CheckerType = CheckerTypes.LDAP,
                CreateMonitorItem = () => new MonitorLDAP() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Memory",
                ItemType = MonitorItemTypes.Memory,
                CheckerType = CheckerTypes.Memory,
                CreateMonitorItem = () => new MonitorMemory() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Ping",
                ItemType = MonitorItemTypes.Ping,
                CheckerType = CheckerTypes.Ping,
                CreateMonitorItem = () => new MonitorPing() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Process running",
                ItemType = MonitorItemTypes.Process,
                CheckerType = CheckerTypes.Process,

                CreateMonitorItem = () => new MonitorProcess() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Registry",
                ItemType = MonitorItemTypes.Registry,
                CheckerType = CheckerTypes.Registry,
                CreateMonitorItem = () => new MonitorRegistry() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "REST API",
                ItemType = MonitorItemTypes.REST,
                CheckerType = CheckerTypes.REST,
                CreateMonitorItem = () => new MonitorREST() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Service",
                ItemType = MonitorItemTypes.Service,
                CheckerType = CheckerTypes.Service,
                CreateMonitorItem = () => new MonitorService() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "SMTP",
                ItemType = MonitorItemTypes.SMTP,
                CheckerType = CheckerTypes.SMTP,
                CreateMonitorItem = () => new MonitorSMTP() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "SOAP",
                ItemType = MonitorItemTypes.SOAP,
                CheckerType = CheckerTypes.SOAP,
                CreateMonitorItem = () => new MonitorSOAP() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "TCP or UDP socket",
                ItemType = MonitorItemTypes.Socket,
                CheckerType = CheckerTypes.Socket,
                CreateMonitorItem = () => new MonitorSocket() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "SQL query",
                ItemType = MonitorItemTypes.SQL,
                CheckerType = CheckerTypes.SQL,
                CreateMonitorItem = () => new MonitorSQL() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "HTTP/HTTPS endpoint",
                ItemType = MonitorItemTypes.URL,
                CheckerType = CheckerTypes.URL,
                CreateMonitorItem = () => new MonitorURL() { ID = Guid.NewGuid().ToString(), Name = "New", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            return list;
        }
    }
}
