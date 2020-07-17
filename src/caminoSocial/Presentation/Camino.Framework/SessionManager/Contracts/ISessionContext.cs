using Camino.Framework.Models;
using System.Threading.Tasks;

namespace Camino.Framework.SessionManager.Contracts
{
    public interface ISessionContext
    {
        string AuthenticationToken { get; }
        Task<ApplicationUser> GetCurrentUserAsync();
    }
}
