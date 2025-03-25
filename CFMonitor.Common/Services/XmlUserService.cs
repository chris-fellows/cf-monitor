using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFWebServer.Services;

namespace CFMonitor.Services
{
    public class XmlUserService : XmlEntityWithIdService<User, string>, IUserService
    {
        private readonly IPasswordService _passwordService;

        public XmlUserService(string folder, IPasswordService passwordService) : base(folder,
                                                "User.*.xml",
                                              (user) => $"User.{user.Id}.xml",
                                                (userId) => $"User.{userId}.xml")
        {
            _passwordService = passwordService;
        }

        public User? ValidateCredentials(string username, string password)
        {
            var user = GetAll().FirstOrDefault(u => u.Email == username);
            if (user != null &&
                user.Active &&
                user.GetUserType() == UserTypes.Normal &&
                _passwordService.IsValid(user.Password, password, user.Salt))
            {
                return user;
            }
            return null;
        }
    }
}
