using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Camino.Infrastructure.AspNetCore.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        { }

        [HttpGet("errors")]
        protected IActionResult RedirectToErrorPage()
        {
            return RedirectToAction("Error", "Home");
        }

        [HttpGet("not-found")]
        protected IActionResult RedirectToNotFoundPage()
        {
            return RedirectToAction("PageNotFound", "Home");
        }

        protected IList<ModelError> GetModelStateErrors()
        {
            return ModelState.Values.SelectMany(x => x.Errors).ToList();
        }
    }
}
