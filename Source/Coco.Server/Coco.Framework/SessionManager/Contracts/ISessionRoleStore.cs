using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface ISessionRoleStore<TRole>
    {
        Task<TRole> FindByNameAsync(string normalizedName);
        void Dispose();
        Task<IList<Claim>> GetClaimsAsync(TRole role);
    }
}
