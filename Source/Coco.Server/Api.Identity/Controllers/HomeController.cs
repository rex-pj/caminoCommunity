using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet, Route("")]
        [EnableCors("AllowOrigin")]
        public IActionResult Index()
        {
            return Content("Identity Api");
        }
    }
}