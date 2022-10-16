using Camino.Framework.Attributes;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Camino.Framework.Controllers
{
    [TokenPopulation]
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
