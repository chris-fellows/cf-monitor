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
                Name = "Active Process",
                Description = "Checks an active process",
                ItemType = MonitorItemTypes.ActiveProcess,
                CheckerType = CheckerTypes.ActiveProcess,
                CreateMonitorItem = () => new MonitorActiveProcess() { ID = Guid.NewGuid().ToString(), 
                            Name = "Active Process [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "DCHP",
                Description = "Checks that DHCP is working",
                ItemType = MonitorItemTypes.DHCP,
                CheckerType = CheckerTypes.DHCP,
                CreateMonitorItem = () => new MonitorDHCP() { ID = Guid.NewGuid().ToString(), 
                            Name = "DHCP [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Disk space",
                Description = "Checks disk space on particular device",
                ItemType = MonitorItemTypes.DiskSpace,
                CheckerType = CheckerTypes.DiskSpace,
                CreateMonitorItem = () => new MonitorDiskSpace() { ID = Guid.NewGuid().ToString(),
                            Name = "Disk space [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "DNS",
                Description = "Checks that DNS is working",
                ItemType = MonitorItemTypes.DNS,
                CheckerType = CheckerTypes.DNS,
                CreateMonitorItem = () => new MonitorDNS() { ID = Guid.NewGuid().ToString(), 
                            Name = "DNS [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Local file",
                Description = "Checks local file exists and optionally contains specific text",
                ItemType = MonitorItemTypes.LocalFile,
                CheckerType = CheckerTypes.LocalFile,
                CreateMonitorItem = () => new MonitorLocalFile() { ID = Guid.NewGuid().ToString(), 
                            Name = "Local file [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "JSON",
                Description = "Checks JSON file",
                ItemType = MonitorItemTypes.JSON,
                CheckerType = CheckerTypes.JSON,
                CreateMonitorItem = () => new MonitorJSON() { ID = Guid.NewGuid().ToString(), 
                            Name = "JSON [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "LDAP",
                Description = "Checks that LDAP is working",
                ItemType = MonitorItemTypes.LDAP,
                CheckerType = CheckerTypes.LDAP,
                CreateMonitorItem = () => new MonitorLDAP() { ID = Guid.NewGuid().ToString(), 
                            Name = "LDAP [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Memory",
                Description = "Checks local memory use",
                ItemType = MonitorItemTypes.Memory,
                CheckerType = CheckerTypes.Memory,
                CreateMonitorItem = () => new MonitorMemory() { ID = Guid.NewGuid().ToString(), 
                            Name = "Memory [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Ping",
                Description = "Pings endpoint",
                ItemType = MonitorItemTypes.Ping,
                CheckerType = CheckerTypes.Ping,
                CreateMonitorItem = () => new MonitorPing() { ID = Guid.NewGuid().ToString(), 
                            Name = "Ping [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });            
            list.Add(new MonitorItemType()
            {
                Name = "Registry",
                Description = "Checks for particular registry setting(s)",
                ItemType = MonitorItemTypes.Registry,
                CheckerType = CheckerTypes.Registry,
                CreateMonitorItem = () => new MonitorRegistry() { ID = Guid.NewGuid().ToString(), 
                            Name = "Registry [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "REST API",
                Description = "Checks REST API returns expected response",
                ItemType = MonitorItemTypes.REST,
                CheckerType = CheckerTypes.REST,
                CreateMonitorItem = () => new MonitorREST() { ID = Guid.NewGuid().ToString(), 
                            Name = "REST API [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Run Process",
                Description = "Runs process and checks the exit code",
                ItemType = MonitorItemTypes.RunProcess,
                CheckerType = CheckerTypes.RunProcess,

                CreateMonitorItem = () => new MonitorRunProcess() { ID = Guid.NewGuid().ToString(), 
                            Name = "Run Process [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "Service",
                Description = "Checks Windows service status",
                ItemType = MonitorItemTypes.Service,
                CheckerType = CheckerTypes.Service,
                CreateMonitorItem = () => new MonitorService() { ID = Guid.NewGuid().ToString(), 
                            Name = "Service [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "SMTP",
                Description = "Checks SMTP connection",
                ItemType = MonitorItemTypes.SMTP,
                CheckerType = CheckerTypes.SMTP,
                CreateMonitorItem = () => new MonitorSMTP() { ID = Guid.NewGuid().ToString(), 
                            Name = "SMTP [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "SOAP",
                Description = "Checks SOAP API returns expected response",
                ItemType = MonitorItemTypes.SOAP,
                CheckerType = CheckerTypes.SOAP,
                CreateMonitorItem = () => new MonitorSOAP() { ID = Guid.NewGuid().ToString(), 
                            Name = "SOAP [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "TCP or UDP socket",
                Description = "Checks TCP or UDP socket",
                ItemType = MonitorItemTypes.Socket,
                CheckerType = CheckerTypes.Socket,
                CreateMonitorItem = () => new MonitorSocket() { ID = Guid.NewGuid().ToString(), 
                            Name = "TCP or UDP socket [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "SQL query",
                Description = "Checks results of SQL query",
                ItemType = MonitorItemTypes.SQL,
                CheckerType = CheckerTypes.SQL,
                CreateMonitorItem = () => new MonitorSQL() { ID = Guid.NewGuid().ToString(), 
                            Name = "SQL Query [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            list.Add(new MonitorItemType()
            {
                Name = "HTTP/HTTPS endpoint",
                Description = "Checks HTTP/HTTPS endpoint returns expected response",
                ItemType = MonitorItemTypes.URL,
                CheckerType = CheckerTypes.URL,
                CreateMonitorItem = () => new MonitorURL() { ID = Guid.NewGuid().ToString(), 
                            Name = "HTTP/HTTPS endpoint [New]", Enabled = true, EventItems = new List<EventItem>(), MonitorItemSchedule = new MonitorItemSchedule() }
            });
            return list;
        }
    }
}
