using Microsoft.AspNetCore.Mvc;

namespace Api.Identity.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet, Route("")]
        public IActionResult Index()
        {
            return Content("Identity Api");
        }
    }
}