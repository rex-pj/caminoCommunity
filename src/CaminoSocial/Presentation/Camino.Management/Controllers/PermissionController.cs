using Camino.Framework.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Camino.Management.Controllers
{
    public class PermissionController : BaseAuthController
    {
        public PermissionController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}