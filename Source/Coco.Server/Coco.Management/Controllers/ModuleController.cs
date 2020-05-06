using Coco.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Management.Controllers
{
    public class ModuleController : BaseAuthController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}