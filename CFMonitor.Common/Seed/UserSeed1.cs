using CFMonitor.EntityReader;
using CFMonitor.Constants;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class UserSeed1 : IEntityReader<User>
    {
        private readonly IPasswordService _passwordService;

        public UserSeed1(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public IEnumerable<User> Read()
        {
            var list = new List<User>()
            {
                new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Test User 1",
                    Email = "testuser1@domain.com",
                    Color = Color.Green.ToArgb().ToString(),
                    ImageSource = "user.png",
                    Password = "testuser1",
                    Role = UserRoleNames.Administrator,
                    Active = true,                    
                },
                new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Test User 2",
                    Email = "testuser2@domain.com",
                    Color = Color.Green.ToArgb().ToString(),
                    ImageSource = "user.png",
                    Password = "testuser2",
                    Role = UserRoleNames.Administrator,
                    Active = true
                },
                new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Test User 3",
                    Email = "testuser3@domain.com",
                    Color = Color.Green.ToArgb().ToString(),
                    ImageSource = "user.png",
                    Password = "testuser3",
                    Role = UserRoleNames.User,
                    Active = true
                }
            };

            // Encrypt passwords
            foreach (var user in list.Where(u => !String.IsNullOrEmpty(u.Password)))
            {
                var encrypted = _passwordService.Encrypt(user.Password);
                user.Password = encrypted[0];
                user.Salt = encrypted[1];
            }

            return list;
        }
    }
}
