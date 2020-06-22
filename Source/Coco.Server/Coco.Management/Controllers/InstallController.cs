using Microsoft.AspNetCore.Mvc;

namespace Coco.Management.Controllers
{
    public class InstallController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Install()
        {
            return View();
        }
    }
}