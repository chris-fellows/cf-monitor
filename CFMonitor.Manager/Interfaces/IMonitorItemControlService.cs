using System.Windows.Forms;
using CFMonitor.Enums;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for service that provides control to display monitor item
    /// </summary>
    public interface IMonitorItemControlService
    {
        UserControl GetControl(MonitorItemTypes monitorItemType);
    }
}
