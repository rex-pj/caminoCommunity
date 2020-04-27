using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.SessionManager.Core;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager
{
    public class SessionClaimsPrincipalFactory<TUser> : ISessionClaimsPrincipalFactory<TUser>
        where TUser : class
    {
        public IUserManager<TUser> UserManager { get; private set; }
        public IdentityOptions Options { get; private set; }

        public SessionClaimsPrincipalFactory(IUserManager<TUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }
            if (optionsAccessor == null || optionsAccessor.Value == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            UserManager = userManager;
            Options = optionsAccessor.Value;
        }

        public virtual async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var id = await GenerateClaimsAsync(user);
            return new ClaimsPrincipal(id);
        }

        protected virtual async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
        {
            var userId = await UserManager.GetUserIdAsync(user);
            var userName = UserManager.GetUserNameAsync(user);
            var email = await UserManager.GetEmailAsync(user);
            var id = new ClaimsIdentity("Identity.Application",
                Options.ClaimsIdentity.UserNameClaimType,
                Options.ClaimsIdentity.RoleClaimType);

            id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, userName));
            id.AddClaim(new Claim(Options.ClaimsIdentity.EmailClaimType, email));
            return id;
        }
    }
}
