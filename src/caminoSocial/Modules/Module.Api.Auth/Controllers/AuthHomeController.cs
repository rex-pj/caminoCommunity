using Microsoft.AspNetCore.Mvc;

namespace Module.Api.Auth.Controllers
{
    public class AuthHomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Content("Auth Api");
        }
    }
}