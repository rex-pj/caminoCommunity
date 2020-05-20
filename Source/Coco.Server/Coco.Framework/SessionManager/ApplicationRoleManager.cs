using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.SessionManager.Stores.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

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
