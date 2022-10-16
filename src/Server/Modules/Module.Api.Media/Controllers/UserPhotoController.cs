using Camino.Application.Contracts.AppServices.Users;
using Camino.Application.Contracts.AppServices.Users.Dtos;
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
    public class UserPhotoController : BaseController
    {
        private readonly IUserPhotoAppService _userPhotoAppService;
        private readonly IUserManager<ApplicationUser> _userManager;
        public UserPhotoController(IUserPhotoAppService userPhotoAppService, IUserManager<ApplicationUser> userManager)
        {
            _userPhotoAppService = userPhotoAppService;
            _userManager = userManager;
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
                var userIdentityId = HttpContext.User.FindFirstValue(HttpHeades.UserIdentityClaimKey);
                var loggedUserId = await _userManager.DecryptUserIdAsync(userIdentityId);

                var fileData = await FileUtils.GetBytesAsync(criterias.File);
                var id = await _userPhotoAppService.UpdateAsync(new UserPhotoUpdateRequest
                {
                    FileName = criterias.FileName,
                    Width = criterias.Width,
                    Height = criterias.Height,
                    Scale = criterias.Scale,
                    UserPhotoTypeId = (int)UserPictureTypes.Avatar,
                    XAxis = criterias.XAxis,
                    YAxis = criterias.YAxis,
                    FileData = fileData,
                }, loggedUserId);

                return Ok(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("covers")]
        [TokenAuthentication]
        public async Task<IActionResult> UpdateCoverAsync(UserPhotoUpdateModel criterias)
        {
            try
            {
                var userIdentityId = HttpContext.User.FindFirstValue(HttpHeades.UserIdentityClaimKey);
                var loggedUserId = await _userManager.DecryptUserIdAsync(userIdentityId);
                var fileData = await FileUtils.GetBytesAsync(criterias.File);
                var id = await _userPhotoAppService.UpdateAsync(new UserPhotoUpdateRequest
                {
                    FileName = criterias.FileName,
                    Width = criterias.Width,
                    Height = criterias.Height,
                    Scale = criterias.Scale,
                    XAxis = criterias.XAxis,
                    YAxis = criterias.YAxis,
                    UserPhotoTypeId = (int)UserPictureTypes.Cover,
                    FileData = fileData
                }, loggedUserId);

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
