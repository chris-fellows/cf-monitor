using CFMonitor.Constants;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class UserSeed1 : IEntityReader<User>
    {
        public Task<List<User>> ReadAllAsync()
        {
            var list = new List<User>()
            {
                new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Test User 1",
                    Email = "testuser1@domain.com",
                    Color = "Blue",
                    ImageSource = "user.png",
                    Password = "testuser1",
                    Role = UserRoleNames.Administrator,
                    Active = true
                },
                new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Test User 2",
                    Email = "testuser2@domain.com",
                    Color = "Blue",
                    ImageSource = "user.png",                    
                    Password = "testuser2",
                    Role = UserRoleNames.Administrator,
                    Active = true
                }
            };

            return Task.FromResult(list);
        }
    }
}
