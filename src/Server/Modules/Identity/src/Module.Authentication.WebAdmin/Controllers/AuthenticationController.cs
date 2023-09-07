using Module.Authentication.WebAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Infrastructure.Identity.Attributes;

namespace Module.Authentication.WebAdmin.Controllers
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
                    return RedirectToAction(nameof(Login));
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View(nameof(Login));
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
