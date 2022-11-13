using Camino.Infrastructure.Identity.Attributes;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Camino.Infrastructure.AspNetCore.Controllers
{
    [TokenIdentityPopulation]
    public class BaseTokenAuthController : BaseController
    {
        protected long LoggedUserId { get; private set; }
        public BaseTokenAuthController(IHttpContextAccessor httpContextAccessor)
        {
            var userIdentityId = httpContextAccessor.HttpContext.User.FindFirstValue(HttpHeaders.UserIdentityClaimKey);
            if (!string.IsNullOrEmpty(userIdentityId))
            {
                var requestServices = httpContextAccessor.HttpContext.RequestServices;
                var userManager = requestServices.GetRequiredService<IUserManager<ApplicationUser>>();
                LoggedUserId = userManager.DecryptUserId(userIdentityId);
            }
        }
    }
}
