using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFWebServer.Services;

namespace CFMonitor.Services
{
    public class XmlUserService : XmlEntityWithIdStoreService<User, string>, IUserService
    {
        public XmlUserService(string folder) : base(folder,
                                                "User.*.xml",
                                              (user) => $"User.{user.Id}.xml",
                                                (userId) => $"User.{userId}.xml")
        {

        }
    }
}
