using Camino.Application.Contracts.AppServices.Users;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Shared.Enums;
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
        public UserPhotoController(IUserPhotoAppService userPhotoAppService)
        {
            _userPhotoAppService = userPhotoAppService;
        }

        [HttpGet("avatars/{code}")]
        public async Task<IActionResult> GetAvatar(string code)
        {
            var avatar = await _userPhotoAppService.GetByCodeAsync(code, UserPictureTypes.Avatar);
            if (avatar == null)
            {
                return NotFound();
            }
            return File(avatar.BinaryData, "image/jpeg");
        }

        [HttpGet("covers/{code}")]
        public async Task<IActionResult> GetCover(string code)
        {
            var cover = await _userPhotoAppService.GetByCodeAsync(code, UserPictureTypes.Cover);
            if (cover == null)
            {
                return NotFound();
            }
            return File(cover.BinaryData, "image/jpeg");
        }

        [HttpPost("avatars")]
        [TokenAuthentication]
        public async Task<IActionResult> UpdateAvatarAsync(UserPhotoUpdateModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var userPrincipalId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                long.TryParse(userPrincipalId, out long loggedUserId);
                var result = await _userPhotoAppService.UpdateAsync(new UserPhotoUpdateRequest
                {
                    PhotoUrl = criterias.PhotoUrl,
                    FileName = criterias.FileName,
                    Width = criterias.Width,
                    Height = criterias.Height,
                    Scale = criterias.Scale,
                    UserPhotoTypeId = (int)UserPictureTypes.Avatar,
                    XAxis = criterias.XAxis,
                    YAxis = criterias.YAxis
                }, loggedUserId);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("covers")]
        [TokenAuthentication]
        public async Task<IActionResult> UpdateCoverAsync(UserPhotoUpdateModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var userPrincipalId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                long.TryParse(userPrincipalId, out long loggedUserId);
                var result = await _userPhotoAppService.UpdateAsync(new UserPhotoUpdateRequest
                {
                    PhotoUrl = criterias.PhotoUrl,
                    FileName = criterias.FileName,
                    Width = criterias.Width,
                    Height = criterias.Height,
                    Scale = criterias.Scale,
                    XAxis = criterias.XAxis,
                    YAxis = criterias.YAxis,
                    UserPhotoTypeId = (int)UserPictureTypes.Cover
                }, loggedUserId);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("avatars")]
        [TokenAuthentication]
        public async Task<IActionResult> DeleteAvatarAsync(PhotoDeleteModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var userPrincipalId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                long.TryParse(userPrincipalId, out long loggedUserId);
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
        public async Task<IActionResult> DeleteCoverAsync(PhotoDeleteModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

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
