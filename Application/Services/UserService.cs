using Application.Constants;
using BCrypt.Net;
using Domains.DTO;
using Domains.DTO.BaseResponse;
using Domains.Interfaces.IGenericRepository;
using Domains.Interfaces.IServices;
using Domains.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(ITokenGenerator tokenGenerator, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _signInManager = signInManager;
        }

        public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            if (loginRequestDTO == null)
                return new ApiResponse<LoginResponseDTO>("Invalid request");

            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (user == null)
                return new ApiResponse<LoginResponseDTO>("Invalid username or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequestDTO.Password, false);
            if (!result.Succeeded)
                return new ApiResponse<LoginResponseDTO>("Invalid username or password");

            var roles = await _userManager.GetRolesAsync(user);
            var tokenValue = _tokenGenerator.GenerateToken(user, roles);
            var userDTO = new LoginResponseDTO(tokenValue, user.Id.ToString(), user.UserName);

            return new ApiResponse<LoginResponseDTO>(userDTO, "Login successful");
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterRequestDTO registerRequestDTO)
        {
            if (registerRequestDTO == null)
                return new ApiResponse<string>("Invalid request");

            var user = new ApplicationUser
            {
                FirstName = registerRequestDTO.FirstName,
                LastName = registerRequestDTO.LastName,
                Email = registerRequestDTO.Email,
                UserName = registerRequestDTO.Email
            };
            var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);
            if (!result.Succeeded)
                return new ApiResponse<string>(string.Join(", ", result.Errors));
            await _userManager.AddToRoleAsync(user, Roles.User);
            return new ApiResponse<string>("User registered successfully");
        }
    }
}
