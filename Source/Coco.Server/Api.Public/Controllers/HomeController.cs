using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet, Route("")]
        public IActionResult Index()
        {
            return Content("Public Api");
        }
    }
}
