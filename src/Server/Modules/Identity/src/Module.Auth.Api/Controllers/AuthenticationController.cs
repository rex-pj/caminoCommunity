using Camino.Infrastructure.Identity.Attributes;
using Camino.Infrastructure.AspNetCore.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Camino.Infrastructure.Identity.Constants;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Infrastructure.Identity.Options;
using Module.Auth.Api.Models;
using Camino.Infrastructure.Identity.Core;
using Microsoft.Extensions.Options;
using Module.Auth.Api.ModelServices;
using System.Linq;
using Camino.Shared.Constants;
using Camino.Core.Validators;
using Module.Auth.Api.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Module.Auth.Api.Controllers
{
    [Route("authentications")]
    public class AuthenticationController : BaseTokenAuthController
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly JwtConfigOptions _jwtConfigOptions;
        private readonly IJwtHelper _jwtHelper;
        private readonly BaseValidatorContext _validatorContext;
        private readonly IAuthenticationModelService _authenticationModelService;

        public AuthenticationController(IUserManager<ApplicationUser> userManager,
            ILoginManager<ApplicationUser> loginManager,
            IJwtHelper jwtHelper,
             BaseValidatorContext validatorContext,
            IOptions<JwtConfigOptions> jwtConfigOptions,
            IAuthenticationModelService authenticationModelService,
            IHttpContextAccessor httpContextAccessor) 
            : base(httpContextAccessor)
        {
            _userManager = userManager;
            _loginManager = loginManager;
            _jwtHelper = jwtHelper;
            _validatorContext = validatorContext;
            _jwtConfigOptions = jwtConfigOptions.Value;
            _authenticationModelService = authenticationModelService;
        }

        [HttpPost("login")]
        [TokenAnnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel criterias)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

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
                return Ok(new UserTokenModel
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

        [HttpPost("refresh-tokens")]
        [TokenAnnonymous]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var authenticationToken = _authenticationModelService.GetAccessTokenFromHeader();
            var clientRefreshToken = _authenticationModelService.GetRefreshTokenFromCookie();
            if (string.IsNullOrWhiteSpace(authenticationToken))
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(clientRefreshToken))
            {
                return Unauthorized();
            }

            var claimsIdentity = await _userManager.GetPrincipalFromExpiredTokenAsync(authenticationToken);
            var userIdentityId = claimsIdentity.Claims.FirstOrDefault(x => x.Type == HttpHeaders.UserIdentityClaimKey).Value;
            if (string.IsNullOrEmpty(userIdentityId))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdentityIdAsync(userIdentityId);
            if (user == null)
            {
                return Unauthorized();
            }

            var serverRefreshToken = await _userManager.GetUserTokenByValueAsync(user, clientRefreshToken, IdentitySettings.AUTHENTICATION_REFRESH_TOKEN_PURPOSE);
            if (serverRefreshToken == null || serverRefreshToken.ExpiryTime <= DateTime.UtcNow)
            {
                return Forbid();
            }

            user.UserIdentityId = userIdentityId;
            var accessToken = _jwtHelper.GenerateJwtToken(user);

            await _userManager.RemoveAuthenticationTokenByValueAsync(user.Id, clientRefreshToken);
            var refreshToken = await _userManager.GenerateUserTokenAsync(user, ServiceProvidersNames.CAMINO_API_AUTH, IdentitySettings.AUTHENTICATION_REFRESH_TOKEN_PURPOSE);
            await _userManager.SetAuthenticationTokenAsync(user, ServiceProvidersNames.CAMINO_API_AUTH, IdentitySettings.AUTHENTICATION_REFRESH_TOKEN_PURPOSE, refreshToken);

            _authenticationModelService.AddRefreshTokenToCookie(refreshToken);
            return Ok(new UserTokenModel
            {
                AuthenticationToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(_jwtConfigOptions.RefreshTokenHourExpires)
            });
        }

        [HttpPatch("update-passwords")]
        [TokenAuthentication]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UserPasswordUpdateModel criterias)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var currentUser = await _userManager.FindByIdAsync(LoggedUserId);
                var result = await _userManager.ChangePasswordAsync(currentUser, criterias.CurrentPassword, criterias.NewPassword);
                if (!result.Succeeded)
                {
                    return Unauthorized();
                }

                return await RefreshTokenAsync();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPost("forgot-password")]
        [TokenAnnonymous]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordModel criterias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _validatorContext.SetValidator(new ForgotPasswordValidator());
            var canUpdate = _validatorContext.Validate<ForgotPasswordModel, bool>(criterias);
            if (!canUpdate)
            {
                return ValidationProblem();
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(criterias.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return Accepted("ForgotPasswordConfirmation");
                }

                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.SetAuthenticationTokenAsync(user, ServiceProvidersNames.CAMINO_API_AUTH, IdentitySettings.RESET_PASSWORD_PURPOSE, resetPasswordToken);
                if (!result.Succeeded)
                {
                    return Problem();
                }

                await _authenticationModelService.SendPasswordChangeAsync(criterias, user, resetPasswordToken);
                return Ok();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPost("reset-password")]
        [TokenAnnonymous]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordModel criterias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(criterias.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new UnauthorizedAccessException("ResetPasswordFailed");
            }

            var result = await _userManager.ResetPasswordAsync(user, criterias.Key, criterias.Password);
            if (!result.Succeeded)
            {
                return Problem();
            }
            await _userManager.RemoveAuthenticationTokenAsync(user, ServiceProvidersNames.CAMINO_API_AUTH, criterias.Key);

            return Ok();
        }
    }
}
