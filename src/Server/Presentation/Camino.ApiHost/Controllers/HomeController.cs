using Microsoft.AspNetCore.Mvc;

namespace Camino.ApiHost.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Camino Api");
        }
    }
}
