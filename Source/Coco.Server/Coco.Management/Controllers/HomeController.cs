using Coco.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Management.Controllers
{
    public class HomeController : Controller
    {
        [AuthenticationSession]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
