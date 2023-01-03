using Camino.Application.Contracts.AppServices.Users;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Application.Validators;
using Camino.Core.Validators;
using Camino.Infrastructure.Identity.Attributes;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Shared.Enums;
using Camino.Shared.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Media.Api.Models;
using System;
using System.Threading.Tasks;

namespace Module.Media.Api.Controllers
{
    [Route("api/user-photos")]
    public class UserPhotoController : BaseTokenAuthController
    {
        private readonly IUserPhotoAppService _userPhotoAppService;
        private readonly BaseValidatorContext _validatorContext;

        public UserPhotoController(IUserPhotoAppService userPhotoAppService,
            BaseValidatorContext validatorContext,
            IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _userPhotoAppService = userPhotoAppService;
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
            return File(avatar.FileData, avatar.ContentType);
        }

        [HttpGet("covers/{id}")]
        public async Task<IActionResult> GetCover(long id)
        {
            var cover = await _userPhotoAppService.GetByIdAsync(id, UserPictureTypes.Cover);
            if (cover == null)
            {
                return NotFound();
            }
            return File(cover.FileData, cover.ContentType);
        }

        [HttpPut("avatars")]
        [TokenAuthentication]
        public async Task<IActionResult> UpdateAvatarAsync([FromForm] UserPhotoUpdateModel criterias, IFormFile file)
        {
            try
            {
                var fileData = await FileUtils.GetBytesAsync(file);
                _validatorContext.SetValidator(new ImageFileValidator());
                bool canUpdate = _validatorContext.Validate<byte[], bool>(fileData);
                if (!canUpdate)
                {
                    return BadRequest(nameof(file));
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
                    ContentType = file.ContentType
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
        public async Task<IActionResult> UpdateCoverAsync(UserPhotoUpdateModel criterias, IFormFile file)
        {
            try
            {
                var fileData = await FileUtils.GetBytesAsync(file);
                _validatorContext.SetValidator(new ImageFileValidator());
                bool canUpdate = _validatorContext.Validate<byte[], bool>(fileData);
                if (!canUpdate)
                {
                    return BadRequest(nameof(file));
                }

                var id = await _userPhotoAppService.UpdateCoverAsync(new UserPhotoUpdateRequest
                {
                    FileName = criterias.FileName,
                    Width = criterias.Width,
                    Height = criterias.Height,
                    Scale = criterias.Scale,
                    XAxis = criterias.XAxis,
                    YAxis = criterias.YAxis,
                    FileData = fileData,
                    ContentType = file.ContentType
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
                await _userPhotoAppService.DeleteByUserIdAsync(LoggedUserId, UserPictureTypes.Avatar);
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
                await _userPhotoAppService.DeleteByUserIdAsync(LoggedUserId, UserPictureTypes.Cover);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
