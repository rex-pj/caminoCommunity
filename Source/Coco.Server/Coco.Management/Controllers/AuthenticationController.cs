using Coco.Framework.Attributes;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using Coco.Management.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Coco.Management.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILoginManager<ApplicationUser> _sessionLoginManager;
        public AuthenticationController(ILoginManager<ApplicationUser> sessionLoginManager)
        {
            _sessionLoginManager = sessionLoginManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _sessionLoginManager.LoginAsync(model.UserName, model.Password);
            if (!result.IsSucceed)
            {
                return RedirectToAction("Login", "Authentication");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AuthenticationSession]
        public async Task<IActionResult> Logout()
        {
            await _sessionLoginManager.LogoutAsync();
            return View("Login");
        }
    }
}
