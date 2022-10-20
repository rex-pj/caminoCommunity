using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Camino.Framework.Models;
using Camino.Infrastructure.Identity.Constants;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Infrastructure.Identity.Options;
using Module.Api.Auth.Models;
using Camino.Infrastructure.Identity.Core;
using Microsoft.Extensions.Options;
using Module.Api.Auth.ModelServices;

namespace Module.Api.Auth.Controllers
{
    [Route("authentications")]
    public class AuthenticationController : BaseController
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly JwtConfigOptions _jwtConfigOptions;
        private readonly IJwtHelper _jwtHelper;
        private readonly IAuthenticationModelService _authenticationModelService;

        public AuthenticationController(IUserManager<ApplicationUser> userManager,
            ILoginManager<ApplicationUser> loginManager,
            IJwtHelper jwtHelper,
            IOptions<JwtConfigOptions> jwtConfigOptions,
            IAuthenticationModelService authenticationModelService)
        {
            _userManager = userManager;
            _loginManager = loginManager;
            _jwtHelper = jwtHelper;
            _jwtConfigOptions = jwtConfigOptions.Value;
            _authenticationModelService = authenticationModelService;
        }

        [HttpPost("login")]
        [TokenAnnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel criterias)
        {
            try
            {
                var result = await _loginManager.PasswordSignInAsync(criterias.Username, criterias.Password, true, true);
                if (!result.Succeeded)
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByNameAsync(criterias.Username);
                user.UserIdentityId = await _userManager.EncryptUserIdAsync(user.Id);
                var accessToken = _jwtHelper.GenerateJwtToken(user);

                var refreshToken = await _userManager.GenerateUserTokenAsync(user, ServiceProvidersNames.CAMINO_API_AUTH, IdentitySettings.AUTHENTICATION_REFRESH_TOKEN_PURPOSE);
                await _userManager.SetAuthenticationTokenAsync(user, ServiceProvidersNames.CAMINO_API_AUTH, IdentitySettings.AUTHENTICATION_REFRESH_TOKEN_PURPOSE, refreshToken);

                _authenticationModelService.AddRefreshTokenToCookie(refreshToken);
                return Ok(new UserTokenModel(true)
                {
                    AuthenticationToken = accessToken,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(_jwtConfigOptions.RefreshTokenHourExpires)
                });
            }
            catch (Exception)
            {
                return Problem();
            }
        }
    }
}
