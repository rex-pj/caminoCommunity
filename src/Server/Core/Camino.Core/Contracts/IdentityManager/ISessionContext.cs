using Camino.Core.Domain.Identities;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.IdentityManager
{
    public interface ISessionContext
    {
        string AuthenticationToken { get; }
        ValueTask<ApplicationUser> GetCurrentUserAsync();
        ValueTask<bool> IsAuthenticatedAsync();
    }
}
