using Domains.Interfaces.IGenericRepository;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(User user, string confirmPassword);
        Task<string> AuthenticateUserAsync(string username, string password);
    }

}
