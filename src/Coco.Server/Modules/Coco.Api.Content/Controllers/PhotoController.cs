using Coco.Business.Contracts;
using Coco.Core.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Coco.Api.Content.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;

        public PhotoController(IUserPhotoBusiness userPhotoBusiness)
        {
            _userPhotoBusiness = userPhotoBusiness;
        }

        // GET cdn/photo/avatar
        [HttpGet]
        [Route("avatar/{code}")]
        public async Task<IActionResult> Avatar(string code)
        {
            var avatar = await _userPhotoBusiness.GetUserPhotoByCodeAsync(code, UserPhotoType.Avatar);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }

        // GET cdn/photo/coverphoto
        [HttpGet]
        [Route("cover/{code}")]
        public async Task<IActionResult> CoverPhoto(string code)
        {
            var avatar = await _userPhotoBusiness.GetUserPhotoByCodeAsync(code, UserPhotoType.Cover);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }
    }
}
