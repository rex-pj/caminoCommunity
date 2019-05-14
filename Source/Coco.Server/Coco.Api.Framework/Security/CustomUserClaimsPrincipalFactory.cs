using Coco.Api.Framework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coco.Api.Framework.Security
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public CustomUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }
       

        /// <summary>
        /// Creates a <see cref="ClaimsPrincipal"/> from an user asynchronously.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsPrincipal"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ClaimsPrincipal"/>.</returns>
        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var id = await GenerateClaimsAsync(user);
            return new ClaimsPrincipal(id);
        }

        /// <summary>
        /// Generate the claims for a user.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsIdentity"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ClaimsIdentity"/>.</returns>
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var userId = await UserManager.GetUserIdAsync(user);
            var userName = await UserManager.GetUserNameAsync(user);
            var id = new ClaimsIdentity("Identity.Application", // REVIEW: Used to match Application scheme
                Options.ClaimsIdentity.UserNameClaimType,
                Options.ClaimsIdentity.RoleClaimType);
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, userName));
            if (UserManager.SupportsUserSecurityStamp)
            {
                id.AddClaim(new Claim(Options.ClaimsIdentity.SecurityStampClaimType,
                    await UserManager.GetSecurityStampAsync(user)));
            }
            if (UserManager.SupportsUserClaim)
            {
                id.AddClaims(await UserManager.GetClaimsAsync(user));
            }
            return id;
        }
    }
}
