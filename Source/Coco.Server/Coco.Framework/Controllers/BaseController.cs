using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coco.Framework.Controllers
{
    public class BaseController : Controller
    {
        protected long LoggedUserId { get; private set; }
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
