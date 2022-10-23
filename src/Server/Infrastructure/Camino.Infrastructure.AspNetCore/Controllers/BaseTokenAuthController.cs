using Camino.Infrastructure.Identity.Attributes;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Camino.Infrastructure.AspNetCore.Controllers
{
    [TokenIdentityPopulation]
    public class BaseTokenAuthController : BaseController
    {
        protected long LoggedUserId { get; private set; }
        public BaseTokenAuthController(IHttpContextAccessor httpContextAccessor)
        {
            var userPrincipalId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            long.TryParse(userPrincipalId, out long loggedUserId);
            LoggedUserId = loggedUserId;
        }
    }
}
