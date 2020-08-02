using Camino.IdentityManager.Models;
using System.Threading.Tasks;

namespace Camino.IdentityManager.Contracts
{
    public interface ISessionContext
    {
        string AuthenticationToken { get; }
        Task<ApplicationUser> GetCurrentUserAsync();
    }
}
