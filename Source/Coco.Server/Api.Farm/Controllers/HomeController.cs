using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Farm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET api/values
        [HttpGet, Route("")]
        [EnableCors("AllowOrigin")]
        public IActionResult Index()
        {
            return Content("Farm Api");
        }
    }
}
