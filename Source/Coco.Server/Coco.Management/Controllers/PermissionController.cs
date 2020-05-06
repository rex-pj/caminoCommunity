using Coco.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Management.Controllers
{
    public class PermissionController : BaseAuthController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}