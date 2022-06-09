using Camino.Infrastructure.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Camino.Infrastructure.Identity
{
    public class ApplicationRoleManager<TRole> : RoleManager<TRole>, IApplicationRoleManager<TRole> 
        where TRole : IdentityRole<long>
    {
        public ApplicationRoleManager(IRoleStore<TRole> store, IEnumerable<IRoleValidator<TRole>> roleValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<TRole>> logger)
            :base(store, roleValidators, keyNormalizer, errors, logger)
        {

        }
    }
}
