using Identity.API.Models;

namespace Identity.API.Services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
    }
}
