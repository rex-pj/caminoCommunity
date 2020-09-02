using Camino.Framework.Attributes;
using Module.Web.AuthenticationManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Models;

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
        public async Task<IActionResult> Login(LoginModel model)
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

        public IActionResult NoAuthorization()
        {
            return View();
        }
    }
}
