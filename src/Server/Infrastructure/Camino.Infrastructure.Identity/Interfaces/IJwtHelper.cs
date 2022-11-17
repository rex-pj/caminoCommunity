using Camino.Infrastructure.Identity.Core;
using System.IdentityModel.Tokens.Jwt;

namespace Camino.Infrastructure.Identity.Interfaces
{
    public interface IJwtHelper
    {
        string GenerateJwtToken(ApplicationUser user);
        JwtSecurityToken GetSecurityToken(string token);
        JwtSecurityToken GetExpiredToken(string token);
    }
}
