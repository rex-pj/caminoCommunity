using Coco.Framework.SessionManager.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Coco.Framework.SessionManager
{
    public class ApplicationRoleManager<TRole> : RoleManager<TRole>, ISessionRoleManager<TRole>
        where TRole : IdentityRole<int>
    {
        public ApplicationRoleManager(IRoleStore<TRole> store, IEnumerable<IRoleValidator<TRole>> roleValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<TRole>> logger)
            :base(store, roleValidators, keyNormalizer, errors, logger)
        {

        }
    }
}
