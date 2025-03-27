using CFMonitor.Interfaces;
using CFMonitor.Models;


namespace CFMonitor.Services
{
    public class XmlNotificationGroupService : XmlEntityWithIdAndNameService<NotificationGroup, string>, INotificationGroupService
    {
        public XmlNotificationGroupService(string folder) : base(folder,
                                                "NotificationGroup.*.xml",
                                                (notificationGroup) => $"NotificationGroup.{notificationGroup.Id}.xml",
                                                (notificationGroupId) => $"NotificationGroup.{notificationGroupId}.xml",
                                                (notificationGroup) => notificationGroup.Name)
        {

        }
    }
}
