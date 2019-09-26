using Coco.Business.Contracts;
using Coco.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Api.Resources.Controllers
{
    [Route("api/[controller]")]
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
        public IActionResult Avatar(string code)
        {
            var avatar = _userPhotoBusiness.GetUserPhotoByCodeAsync(code, UserPhotoTypeEnum.Avatar);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }

        // GET cdn/photo/coverphoto
        [HttpGet]
        [Route("cover/{code}")]
        public IActionResult CoverPhoto(string code)
        {
            var avatar = _userPhotoBusiness.GetUserPhotoByCodeAsync(code, UserPhotoTypeEnum.Cover);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }
    }
}
