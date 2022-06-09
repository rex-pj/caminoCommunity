using Camino.Infrastructure.Identity.Core;
using System.Security.Claims;

namespace Camino.Infrastructure.Identity.Interfaces
{
    public interface IJwtHelper
    {
        Task<ClaimsIdentity> ValidateTokenAsync(string token);
        string GenerateJwtToken(ApplicationUser user);
        Task<ClaimsIdentity> GetPrincipalFromExpiredTokenAsync(string token);
    }
}
