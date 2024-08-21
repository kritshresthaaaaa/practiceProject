using Application.Constants;
using Domains.DTO;
using Domains.Interfaces.IServices;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserValidator : IUserValidator
    {
        private static readonly List<User> Users = new List<User>
        {
            new User { Username = "admin", Password = "admin123", Roles = new[] { Roles.Admin } },
            new User { Username = "user", Password = "user123", Roles = new[] { Roles.User } }
        };

        public User ValidateUser(LoginRequestDTO loginRequestDTO)
        {
            return Users.SingleOrDefault(u => u.Username == loginRequestDTO.Username && u.Password == loginRequestDTO.Password);
        }
    }
}
