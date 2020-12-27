using Camino.IdentityManager.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Camino.IdentityManager
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
