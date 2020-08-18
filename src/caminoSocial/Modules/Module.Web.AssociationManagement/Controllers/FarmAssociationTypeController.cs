using Camino.Framework.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Module.Web.AssociationManagement.Controllers
{
    public class FarmAssociationTypeController : BaseAuthController
    {
        public FarmAssociationTypeController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

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