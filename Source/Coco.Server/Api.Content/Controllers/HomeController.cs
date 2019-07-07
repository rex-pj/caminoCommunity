using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Content.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET api/user
        [HttpGet, Route("")]
        [EnableCors("AllowOrigin")]
        public IActionResult Index()
        {
            return Content("Content Api");
        }
    }
}
