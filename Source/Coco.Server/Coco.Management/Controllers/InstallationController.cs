using Microsoft.AspNetCore.Mvc;

namespace Coco.Management.Controllers
{
    public class InstallationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}