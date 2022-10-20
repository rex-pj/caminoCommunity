using Camino.Application.Contracts.AppServices.Users;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Application.Validators;
using Camino.Core.Validators;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Constants;
using Camino.Shared.Enums;
using Camino.Shared.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Api.Media.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Media.Controllers
{
    [Route("user-photos")]
    public class UserPhotoController : BaseTokenAuthController
    {
        private readonly IUserPhotoAppService _userPhotoAppService;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly BaseValidatorContext _validatorContext;

        public UserPhotoController(IUserPhotoAppService userPhotoAppService,
            IUserManager<ApplicationUser> userManager,
            BaseValidatorContext validatorContext,
            IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _userPhotoAppService = userPhotoAppService;
            _userManager = userManager;
            _validatorContext = validatorContext;
        }

        [HttpGet("avatars/{id}")]
        public async Task<IActionResult> GetAvatar(long id)
        {
            var avatar = await _userPhotoAppService.GetByIdAsync(id, UserPictureTypes.Avatar);
            if (avatar == null)
            {
                return NotFound();
            }
            return File(avatar.FileData, "image/jpeg");
        }

        [HttpGet("covers/{id}")]
        public async Task<IActionResult> GetCover(long id)
        {
            var cover = await _userPhotoAppService.GetByIdAsync(id, UserPictureTypes.Cover);
            if (cover == null)
            {
                return NotFound();
            }
            return File(cover.FileData, "image/jpeg");
        }

        [HttpPut("avatars")]
        [TokenAuthentication]
        public async Task<IActionResult> UpdateAvatarAsync([FromForm] UserPhotoUpdateModel criterias)
        {
            try
            {
                var fileData = await FileUtils.GetBytesAsync(criterias.File);
                _validatorContext.SetValidator(new ImageFileValidator());
                bool canUpdate = _validatorContext.Validate<byte[], bool>(fileData);
                if (!canUpdate)
                {
                    return BadRequest(nameof(criterias.File));
                }

                var id = await _userPhotoAppService.UpdateAvatarAsync(new UserPhotoUpdateRequest
                {
                    FileName = criterias.FileName,
                    Width = criterias.Width,
                    Height = criterias.Height,
                    Scale = criterias.Scale,
                    XAxis = criterias.XAxis,
                    YAxis = criterias.YAxis,
                    FileData = fileData,
                }, LoggedUserId);

                return Ok(id);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [HttpPut("covers")]
        [TokenAuthentication]
        public async Task<IActionResult> UpdateCoverAsync(UserPhotoUpdateModel criterias)
        {
            try
            {
                var fileData = await FileUtils.GetBytesAsync(criterias.File);
                _validatorContext.SetValidator(new ImageFileValidator());
                bool canUpdate = _validatorContext.Validate<byte[], bool>(fileData);
                if (!canUpdate)
                {
                    return BadRequest(nameof(criterias.File));
                }

                var id = await _userPhotoAppService.UpdateCoverAsync(new UserPhotoUpdateRequest
                {
                    FileName = criterias.FileName,
                    Width = criterias.Width,
                    Height = criterias.Height,
                    Scale = criterias.Scale,
                    XAxis = criterias.XAxis,
                    YAxis = criterias.YAxis,
                    FileData = fileData
                }, LoggedUserId);

                return Ok(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("avatars")]
        [TokenAuthentication]
        public async Task<IActionResult> DeleteAvatarAsync()
        {
            try
            {
                var userIdentityId = HttpContext.User.FindFirstValue(HttpHeades.UserIdentityClaimKey);
                var loggedUserId = await _userManager.DecryptUserIdAsync(userIdentityId);
                await _userPhotoAppService.DeleteByUserIdAsync(loggedUserId, UserPictureTypes.Avatar);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("covers")]
        [TokenAuthentication]
        public async Task<IActionResult> DeleteCoverAsync()
        {
            try
            {
                var userPrincipalId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                long.TryParse(userPrincipalId, out long loggedUserId);
                await _userPhotoAppService.DeleteByUserIdAsync(loggedUserId, UserPictureTypes.Cover);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
