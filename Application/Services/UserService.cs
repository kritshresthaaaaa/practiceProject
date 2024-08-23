using Application.Constants;
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

        public UserService(ITokenGenerator tokenGenerator, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            if (loginRequestDTO == null)
                return new ApiResponse<LoginResponseDTO>("Invalid request");

            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
            if (user == null)
                return new ApiResponse<LoginResponseDTO>("Invalid email or password");

            var result = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (!result)
                return new ApiResponse<LoginResponseDTO>("Invalid email or password");

            var roles = await _userManager.GetRolesAsync(user);
            var tokenValue = _tokenGenerator.GenerateToken(user, roles);
            var userDTO = new LoginResponseDTO(tokenValue, user.Id.ToString(), user.UserName);
            return new ApiResponse<LoginResponseDTO>(userDTO, "Login successful");
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterRequestDTO registerRequestDTO)
        {
            if (registerRequestDTO == null)
                return new ApiResponse<string>("Invalid request");
            if (registerRequestDTO.Password != registerRequestDTO.ConfirmPassword)
            {
                return new ApiResponse<string>("Passwords do not match");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
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
                    await _unitOfWork.RollbackTransactionAsync();
                    var errorDescriptions = result.Errors.Select(error => error.Description).ToList();
                    var errorMessage = string.Join(", ", errorDescriptions); // Joining error messages  with a comma separator 
                    return new ApiResponse<string>(errorMessage);
                }

                var roleResult = await _userManager.AddToRoleAsync(user, Roles.User);
                if (!roleResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ApiResponse<string>(string.Join(", ", roleResult.Errors));
                }
                await _unitOfWork.CommitTransactionAsync();
                return new ApiResponse<string>(null, "User registered successfully");
            }
            catch (DbUpdateException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiResponse<string>(ex.Message);
            }
        }
    }
}
