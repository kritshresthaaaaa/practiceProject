using System.Security.Claims;
using Domains.Interfaces.IServices;
using Microsoft.AspNetCore.Http;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value ?? string.Empty;
        }
    }

    public string Username
    {
        get
        {
            var usernameClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name);
            return usernameClaim?.Value ?? string.Empty;
        }
    }
    public string[] Roles
    {
        get
        {
            var rolesClaims = _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role);
            return rolesClaims?.Select(r => r.Value).ToArray() ?? Array.Empty<string>();
        }
    }
}
