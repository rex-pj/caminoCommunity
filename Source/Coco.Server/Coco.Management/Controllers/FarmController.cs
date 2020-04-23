using Microsoft.AspNetCore.Mvc;

namespace Coco.Management.Controllers
{
    public class FarmController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}