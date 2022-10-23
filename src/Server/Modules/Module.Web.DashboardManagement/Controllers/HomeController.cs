using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Infrastructure.Identity.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Module.Web.DashboardManagement.Controllers
{
    public class HomeController : BaseAuthController
    {
        public HomeController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }


        [ApplicationAuthentication]
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
