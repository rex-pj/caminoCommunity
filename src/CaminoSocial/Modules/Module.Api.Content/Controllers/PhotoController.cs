using Camino.Business.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserPhotoKind = Camino.Data.Enums.UserPhotoKind;

namespace Module.Api.Content.Controllers
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
            var avatar = await _userPhotoBusiness.GetUserPhotoByCodeAsync(code, UserPhotoKind.Avatar);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }

        // GET cdn/photo/coverphoto
        [HttpGet]
        [Route("cover/{code}")]
        public async Task<IActionResult> CoverPhoto(string code)
        {
            var avatar = await _userPhotoBusiness.GetUserPhotoByCodeAsync(code, UserPhotoKind.Cover);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }
    }
}
