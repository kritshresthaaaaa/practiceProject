using Application.Constants;
using Application.Exceptions;
using Application.Extensions;
using BCrypt.Net;
using Domains.DTO;
using Domains.DTO.BaseResponse;
using Domains.Interfaces.IGenericRepository;
using Domains.Interfaces.IServices;
using Domains.Interfaces.IUnitofWork;
using Domains.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
 

        public UserService(ILogger<UserService> logger, ITokenGenerator tokenGenerator, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _unitOfWork = unitOfWork;
            _logger = logger;

        }
        public async Task<ApplicationUser> GetUserById(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new BadRequestException("Invalid userId");
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return user;
        }
        public async Task ConfirmEmailAsync(ApplicationUser user, string token)
        {
            if (user == null || token == null)
            {
                throw new BadRequestException("Invalid request");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var errorDescriptions = result.Errors.Select(error => error.Description).ToList();
                var errorMessage = string.Join(", ", errorDescriptions);
                throw new BadRequestException(errorMessage);
            }
        }

        public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            if (loginRequestDTO == null)
                return new ApiResponse<LoginResponseDTO>("Invalid request");

            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
            if (user == null)
                return new ApiResponse<LoginResponseDTO>("Invalid email or password");
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                var lockoutTime = user.LockoutEnd - DateTime.UtcNow;
                return new ApiResponse<LoginResponseDTO>($"User is locked out for {lockoutTime.Value.Minutes} minutes");
            }
            var result = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (!result)
            {
                await HandleFailedLoginAttempt(user);
                return new ApiResponse<LoginResponseDTO>("Invalid email or password");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokenValue = _tokenGenerator.GenerateToken(user, roles);
            var userDTO = new LoginResponseDTO(tokenValue, user.Id.ToString(), user.UserName!);
            return new ApiResponse<LoginResponseDTO>(userDTO, "Login successful");
        }
        private async Task HandleFailedLoginAttempt(ApplicationUser user)
        {
            if (user == null)
                return;

            user.AccessFailedCount++;
            if (user.AccessFailedCount >= 5)
            {
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(30);
                user.AccessFailedCount = 0;
                
            }
            await _userManager.UpdateAsync(user);
        }
        public async Task<ApiResponse<string>> RegisterAsync(RegisterRequestDTO registerRequestDTO)
        {
            if (registerRequestDTO == null)
                return new ApiResponse<string>("Invalid request");

            if (registerRequestDTO.Password != registerRequestDTO.ConfirmPassword)
            {
                return new ApiResponse<string>("Passwords do not match");
            }

            var user = new ApplicationUser
            {
                FirstName = registerRequestDTO.FirstName,
                LastName = registerRequestDTO.LastName,
                Email = registerRequestDTO.Email,
                UserName = registerRequestDTO.Email,
                City = registerRequestDTO.City,
                Address = registerRequestDTO.Address,
            };

            var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);
            if (!result.Succeeded)
            {
                var errorDescriptions = result.Errors.Select(error => error.Description).ToList();
                var errorMessage = string.Join(", ", errorDescriptions); // Joining error messages with a comma separator
                return new ApiResponse<string>(errorMessage);
            }

            // generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _logger.LogInformation("Email confirmation token: " + token);

            var roleResult = await _userManager.AddToRoleAsync(user, Roles.User);
            if (!roleResult.Succeeded)
            {
                return new ApiResponse<string>(string.Join(", ", roleResult.Errors));
            }

            return new ApiResponse<string>(null, "User registered successfully");
        }
    }
}
