using Camino.Infrastructure.Identity.Attributes;
using Camino.Infrastructure.AspNetCore.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Camino.Infrastructure.Identity.Interfaces;
using Module.Auth.Api.Models;
using Camino.Infrastructure.Identity.Core;
using Module.Auth.Api.ModelServices;
using Camino.Shared.Enums;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Application.Contracts;
using Camino.Infrastructure.AspNetCore.Models;
using Microsoft.AspNetCore.Http;
using Camino.Core.Validators;
using Module.Auth.Api.Validators;
using Camino.Application.Contracts.AppServices.Users;
using System.Linq;

namespace Module.Auth.Api.Controllers
{
    [Route("api/users")]
    public class UserController : BaseTokenAuthController
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IUserModelService _userModelService;
        private readonly BaseValidatorContext _validatorContext;
        private readonly IUserAppService _userAppService;

        public UserController(IUserManager<ApplicationUser> userManager,
            BaseValidatorContext validatorContext,
            IUserModelService userModelService,
            IUserAppService userAppService,
            IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _userManager = userManager;
            _userAppService = userAppService;
            _validatorContext = validatorContext;
            _userModelService = userModelService;
        }

        [HttpPost("registration")]
        [TokenAnnonymous]
        public async Task<IActionResult> Register([FromBody] SignupModel criterias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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

        [HttpPatch("partials")]
        [TokenAuthentication]
        public async Task<IActionResult> PartialUpdateAsync([FromBody] PartialUpdateModel criterias)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _validatorContext.SetValidator(new PartialUpdateValidator());
                var canUpdate = _validatorContext.Validate<PartialUpdateModel, bool>(criterias);
                if (!canUpdate)
                {
                    return ValidationProblem(ModelState);
                }

                var userId = await _userManager.DecryptUserIdAsync(criterias.Key.ToString());
                if (userId != LoggedUserId)
                {
                    throw new UnauthorizedAccessException();
                }

                var updatedItem = await _userAppService.PartialUpdateAsync(new PartialUpdateRequest
                {
                    Key = userId,
                    Updates = criterias.Updates.Select(x => new PartialUpdateItemRequest
                    {
                        PropertyName = x.PropertyName,
                        Value = x.Value
                    }).ToList()
                });

                return Ok();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPut("identifiers")]
        [TokenAuthentication]
        public async Task<IActionResult> UpdateIdentifierAsync([FromBody] UserIdentifierUpdateModel criterias)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var currentUser = await _userManager.FindByIdAsync(LoggedUserId);
                currentUser.Lastname = criterias.Lastname;
                currentUser.Firstname = criterias.Firstname;
                currentUser.DisplayName = criterias.DisplayName;

                var updatedUser = await _userManager.UpdateAsync(currentUser);
                if (updatedUser.Succeeded)
                {
                    return Ok(new UserIdentifierUpdateRequest()
                    {
                        DisplayName = currentUser.DisplayName,
                        Firstname = currentUser.Firstname,
                        Id = currentUser.Id,
                        Lastname = currentUser.Lastname
                    });
                }

                return Problem();
            }
            catch (Exception)
            {
                return Problem();
            }
        }
    }
}
