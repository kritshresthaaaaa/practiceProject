using Application.Constants;
using BCrypt.Net;
using Domains.DTO;
using Domains.DTO.BaseResponse;
using Domains.Interfaces.IGenericRepository;
using Domains.Interfaces.IServices;
using Domains.Models;
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
        private readonly IUserValidator _userValidator;
        private readonly ITokenGenerator _tokenGenerator;

        public UserService(IUserValidator userValidator, ITokenGenerator tokenGenerator)
        {
            _userValidator = userValidator;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            if (loginRequestDTO == null)
                return new ApiResponse<LoginResponseDTO>("Invalid request");

            var user = _userValidator.ValidateUser(loginRequestDTO);
            if (user == null)
                return new ApiResponse<LoginResponseDTO>("Invalid username or password");

            var tokenValue = _tokenGenerator.GenerateToken(user);
            var userDTO = new LoginResponseDTO(tokenValue, user.Id.ToString(), user.Username);

            return new ApiResponse<LoginResponseDTO>(userDTO, "Login successful");
        }
    }
}
