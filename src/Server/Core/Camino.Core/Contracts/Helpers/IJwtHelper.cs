using Camino.Core.Domain.Identities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Helpers
{
    public interface IJwtHelper
    {
        Task<ClaimsIdentity> ValidateTokenAsync(string token);
        string GenerateJwtToken(ApplicationUser user);
    }
}
