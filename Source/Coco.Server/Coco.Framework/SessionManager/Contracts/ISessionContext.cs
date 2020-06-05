using Coco.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface ISessionContext
    {
        string AuthenticationToken { get; }
        Task<ApplicationUser> GetCurrentUserAsync();
    }
}
