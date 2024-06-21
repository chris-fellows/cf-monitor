using CFMonitor.Controls;
using CFMonitor.Enums;
using CFMonitor.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CFMonitor.Services
{
    internal class MonitorItemControlService : IMonitorItemControlService
    {
        public UserControl GetControl(MonitorItemTypes monitorItemType)
        {
            // TODO: Consider doing this via reflection. Add MonitorItemType property to user control
            var controlCreators = new Dictionary<MonitorItemTypes, Func<UserControl>>()
            {
                { MonitorItemTypes.ActiveProcess, () => new MonitorActiveProcessControl() },
                { MonitorItemTypes.CPU, () => new MonitorCPUControl() },
                { MonitorItemTypes.DHCP, () => new MonitorDHCPControl() },
                { MonitorItemTypes.DiskSpace, () => new MonitorDiskSpaceControl() },
                { MonitorItemTypes.DNS, () => new MonitorDNSControl() },
                { MonitorItemTypes.FileSize, () => new MonitorFileSizeControl() },
                { MonitorItemTypes.FolderSize, () => new MonitorFolderSizeControl() },
                { MonitorItemTypes.IMAP, () => new MonitorIMAPControl() },
                { MonitorItemTypes.JSON, () => null },
                { MonitorItemTypes.LDAP, () => new MonitorLDAPControl() },
                { MonitorItemTypes.LocalFile, () => new MonitorLocalFileControl() },
                { MonitorItemTypes.Memory, () => new MonitorMemoryControl() },
                { MonitorItemTypes.NTP, () => new MonitorNTPControl() },
                { MonitorItemTypes.Ping, () => new MonitorPingControl() },
                { MonitorItemTypes.POP, () => new MonitorPOPControl() },
                { MonitorItemTypes.Registry, () => new MonitorRegistryControl() },
                { MonitorItemTypes.REST, () => null },
                { MonitorItemTypes.RunProcess, () => new MonitorRunProcessControl() },
                { MonitorItemTypes.Service, () => new MonitorServiceControl() },
                { MonitorItemTypes.SMTP, () => new MonitorSMTPControl() },
                { MonitorItemTypes.SOAP, () => null },
                { MonitorItemTypes.Socket, () => new MonitorSocketControl() },
                { MonitorItemTypes.SQL, () => new MonitorServiceControl() },
                { MonitorItemTypes.URL, () => new MonitorURLControl() }                
            };
            return controlCreators[monitorItemType]();

            /*
            return monitorItemType switch
            {
                MonitorItemTypes.SOAP => null,
                _ => null
            };
            */
        }
    }
}
