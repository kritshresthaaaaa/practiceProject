using BCrypt.Net;
using Domains.Interfaces.IGenericRepository;
using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _genericRepository;
        private readonly TokenService _tokenService;

        public UserService(IGenericRepository<User> genericRepository, TokenService tokenService)
        {
            _genericRepository = genericRepository;
            _tokenService = tokenService;
        }

        public async Task<string> RegisterUserAsync(User user, string confirmPassword)
        {
            if (user.Password != confirmPassword)
                return "Password and Confirm Password do not match";

            var userQuery = _genericRepository.GetQueryable().Where(u => u.Username == user.Username);
            var userExists = await userQuery.AnyAsync();

            if (userExists)
                return "User already exists";

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _genericRepository.AddAsync(user);

            return "User registered successfully";
        }

        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            var user = await _genericRepository.GetQueryable().FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            return _tokenService.GenerateJwtToken(user);
        }
    }
}
