using Microsoft.AspNetCore.Mvc;

namespace Camino.Infrastructure.AspNetCore.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        { }

        [HttpGet("errors")]
        public IActionResult RedirectToErrorPage()
        {
            return RedirectToAction("Error", "Home");
        }

        [HttpGet("not-found")]
        public IActionResult RedirectToNotFoundPage()
        {
            return RedirectToAction("PageNotFound", "Home");
        }
    }
}
