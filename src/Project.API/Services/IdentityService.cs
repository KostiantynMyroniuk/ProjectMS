using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Project.API.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContext;

        public IdentityService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string GetUserId()
        {
            return _httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetUserName()
        {
            return _httpContext.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value
                ?? _httpContext.HttpContext?.User.FindFirst("name")?.Value
                ?? _httpContext.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
        }

        public string GetEmail()
        {
            return _httpContext.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value
                ?? _httpContext.HttpContext?.User.FindFirst("email")?.Value
                ?? _httpContext.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
        }
    }
}
