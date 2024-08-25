using Domains.DTO;
using Domains.DTO.BaseResponse;
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
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDTORequest resetPasswordDTO);

        Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<ApiResponse<string>> RegisterAsync(RegisterRequestDTO registerRequestDTO);
        Task<ApplicationUser> GetUserById(Guid userId);
        Task ConfirmEmailAsync(ConfirmEmailRequestDTO confirmEmailRequestDTO);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordDTORequest forgotPasswordDTORequest);
    }
}
