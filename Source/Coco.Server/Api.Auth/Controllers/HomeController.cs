using Microsoft.AspNetCore.Mvc;

namespace Api.Auth.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet, Route("")]
        public IActionResult Index()
        {
            return Content("Auth Api");
        }
    }
}