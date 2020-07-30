using Camino.Framework.Attributes;
using Camino.Framework.Models;
using Camino.Framework.SessionManager.Contracts;
using Module.Web.AuthenticationManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace Module.Web.AuthenticationManagement.Controllers
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
            try
            {
                var result = await _loginManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
                if (!result.Succeeded)
                {
                    return RedirectToAction("Login", "Authentication");
                }

                return RedirectToAction("Index", "Home");
            }
            catch(Exception e)
            {
                return await Login(model);
            }
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
