using Microsoft.AspNetCore.Mvc;

namespace Api.Resource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Content("Resource Page");
        }
    }
}