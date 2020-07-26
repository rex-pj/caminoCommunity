using Microsoft.AspNetCore.Mvc;

namespace Module.Api.Content.Controllers
{
    public class ContentHomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Content("Content Api");
        }
    }
}
