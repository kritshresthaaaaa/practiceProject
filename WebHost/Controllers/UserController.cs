using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Services;
using Domains.Models;
using Domains.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Constants;
using Application.Exceptions;
using WebHost.DTO.BaseResponse; // Assuming User model and LoginRequestDTO are defined here

namespace WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var response = await _userService.LoginAsync(loginRequestDTO);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var response = await _userService.RegisterAsync(registerRequestDTO);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpPost]
        [Route("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestDTO confirmEmailRequestDTO)
        {
            if (confirmEmailRequestDTO.UserId == Guid.Empty || confirmEmailRequestDTO.Token == null)
            {
                return BadRequest("Invalid request");
            }
            await _userService.ConfirmEmailAsync(confirmEmailRequestDTO);
            return Ok(new ApiResponse<string>(null, "Email confirmed successfully"));
        }
        [HttpPost]
        [Route("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTORequest forgotPasswordRequestDTO)
        {
            if (forgotPasswordRequestDTO == null)
            {
                return BadRequest("Invalid request");
            }
            var response = await _userService.ForgotPasswordAsync(forgotPasswordRequestDTO);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }
        [HttpPost]

        [Route("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTORequest resetPasswordDTORequest)
        {
            if (resetPasswordDTORequest == null)
            {
                return BadRequest("Invalid request");
            }
            var response = await _userService.ResetPasswordAsync(resetPasswordDTORequest);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }


    }

}
