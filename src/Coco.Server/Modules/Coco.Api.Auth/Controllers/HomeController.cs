using Microsoft.AspNetCore.Mvc;

namespace  Coco.Api.Auth.Controllers
{
    [Route("api/[controller]")]
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