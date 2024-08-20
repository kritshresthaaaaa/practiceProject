using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Services;
using Domains.Models;
using Domains.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text; // Assuming User model and LoginRequestDTO are defined here

namespace WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static readonly List<User> Users = new List<User>
        {
            new User { Username = "admin", Password = "admin123", Roles = new[] { "Admin" } },
            new User { Username = "user", Password = "user123", Roles = new[] { "User" } }
        };

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            if (loginRequestDTO == null)
                return BadRequest("Invalid request");

            var user = Users.SingleOrDefault(u => u.Username == loginRequestDTO.Username && u.Password == loginRequestDTO.Password);
            if (user == null)
                return Unauthorized("Invalid username or password");
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId",user.Id.ToString()),
                new Claim("Username", user.Username),
                new Claim(ClaimTypes.Role, user.Roles.FirstOrDefault())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(

                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signin
            );
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { Token = tokenValue, User = user });
        }

    }

}
