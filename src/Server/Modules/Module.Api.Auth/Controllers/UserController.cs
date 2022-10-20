using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Camino.Infrastructure.Identity.Interfaces;
using Module.Api.Auth.Models;
using Camino.Infrastructure.Identity.Core;
using Module.Api.Auth.ModelServices;
using Camino.Shared.Enums;

namespace Module.Api.Auth.Controllers
{
    [Route("users")]
    public class UserController : BaseController
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IUserModelService _userModelService;

        public UserController(IUserManager<ApplicationUser> userManager,
            IUserModelService userModelService)
        {
            _userManager = userManager;
            _userModelService = userModelService;
        }

        [HttpPost("registration")]
        [TokenAnnonymous]
        public async Task<IActionResult> Register([FromBody] SignupModel criterias)
        {
            var user = new ApplicationUser()
            {
                BirthDate = criterias.BirthDate,
                DisplayName = $"{criterias.Lastname} {criterias.Firstname}",
                Email = criterias.Email,
                Firstname = criterias.Firstname,
                Lastname = criterias.Lastname,
                GenderId = (int)criterias.GenderId,
                StatusId = (int)UserStatuses.Pending,
                UserName = criterias.Email,
            };
            try
            {
                var result = await _userManager.CreateAsync(user, criterias.Password);
                if (!result.Succeeded)
                {
                    return Problem();
                }

                user = await _userManager.FindByNameAsync(user.UserName);
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userModelService.SendActiveEmailAsync(user, confirmationToken);
                return Ok(user.Id);
            }
            catch (Exception)
            {
                return Problem();
            }
        }        
    }
}
