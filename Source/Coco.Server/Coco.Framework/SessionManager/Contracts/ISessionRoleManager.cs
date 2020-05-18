using Coco.Framework.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface ISessionRoleManager<TRole> where TRole : IdentityRole<int>
    {
        //Task<ApplicationRole> FindByNameAsync(string roleName);
        //Task<IList<Claim>> GetClaimsAsync(ApplicationRole role);
    }
}
