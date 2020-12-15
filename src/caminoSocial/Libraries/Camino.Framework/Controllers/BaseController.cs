using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Camino.Framework.Controllers
{
    public class BaseController : Controller
    {
        protected long LoggedUserId { get; private set; }
        protected string FeatureName { get; set; }
        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            var userPrincipalId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            LoggedUserId = long.Parse(userPrincipalId);
        }

        public IActionResult RedirectToErrorPage()
        {
            return RedirectToAction("Error", "Home");
        }

        public IActionResult RedirectToNotFoundPage()
        {
            return RedirectToAction("PageNotFound", "Home");
        }
    }
}
