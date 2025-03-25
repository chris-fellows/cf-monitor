using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    public interface IUserService : IEntityWithIdService<User, string>
    {
        User? ValidateCredentials(string username, string password);
    }
}
