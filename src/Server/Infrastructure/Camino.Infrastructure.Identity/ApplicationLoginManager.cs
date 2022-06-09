using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;

namespace Camino.Infrastructure.Identity
{
    public class ApplicationLoginManager<TUser> : SignInManager<TUser>, ILoginManager<TUser> where TUser : ApplicationUser
    {
        public ApplicationLoginManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes,
            IUserConfirmation<TUser> confirmation) :
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }
    }
}
