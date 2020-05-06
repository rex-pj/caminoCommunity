using Coco.Framework.Attributes;
using Coco.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Management.Controllers
{
    public class HomeController : BaseAuthController
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

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
