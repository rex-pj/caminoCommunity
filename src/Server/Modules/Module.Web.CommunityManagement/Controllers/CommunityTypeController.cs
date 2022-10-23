using Camino.Infrastructure.AspNetCore.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Module.Web.CommunityManagement.Controllers
{
    public class CommunityTypeController : BaseAuthController
    {
        public CommunityTypeController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
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