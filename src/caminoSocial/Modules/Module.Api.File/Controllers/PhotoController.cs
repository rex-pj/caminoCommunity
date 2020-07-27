using Camino.Business.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Camino.Data.Enums;

namespace Module.Api.File.Controllers
{
    [Route("[controller]")]
    public class PhotoController : Controller
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;

        public PhotoController(IUserPhotoBusiness userPhotoBusiness)
        {
            _userPhotoBusiness = userPhotoBusiness;
        }

        [HttpGet]
        [Route("avatar/{code}")]
        public async Task<IActionResult> Avatar(string code)
        {
            var avatar = await _userPhotoBusiness.GetUserPhotoByCodeAsync(code, UserPhotoKind.Avatar);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }

        [HttpGet]
        [Route("cover/{code}")]
        public async Task<IActionResult> Cover(string code)
        {
            var avatar = await _userPhotoBusiness.GetUserPhotoByCodeAsync(code, UserPhotoKind.Cover);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }
    }
}
