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
        private readonly ILoginManager<ApplicationUser> _loginManager;
        public AuthenticationController(ILoginManager<ApplicationUser> loginManager)
        {
            _loginManager = loginManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _loginManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
            if (!result.Succeeded)
            {
                return RedirectToAction("Login", "Authentication");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [ApplicationAuthentication]
        public async Task<IActionResult> Logout()
        {
            await _loginManager.SignOutAsync();
            return View("Login");
        }
    }
}
