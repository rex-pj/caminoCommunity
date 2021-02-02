using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Camino.Data.Enums;
using Camino.Service.Business.Users.Contracts;
using Camino.Service.Business.Media.Contracts;

namespace Module.Api.Media.Controllers
{
    [Route("file-api/[controller]")]
    public class PhotoController : Controller
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;
        private readonly IPictureBusiness _pictureBusiness;

        public PhotoController(IUserPhotoBusiness userPhotoBusiness, IPictureBusiness pictureBusiness)
        {
            _userPhotoBusiness = userPhotoBusiness;
            _pictureBusiness = pictureBusiness;
        }

        [HttpGet]
        [Route("avatar/{code}")]
        public async Task<IActionResult> GetAvatar(string code)
        {
            var avatar = await _userPhotoBusiness.GetUserPhotoByCodeAsync(code, UserPhotoKind.Avatar);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }

        [HttpGet]
        [Route("cover/{code}")]
        public async Task<IActionResult> GetCover(string code)
        {
            var avatar = await _userPhotoBusiness.GetUserPhotoByCodeAsync(code, UserPhotoKind.Cover);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }

        [HttpGet]
        [IgnoreAntiforgeryToken]
        [Route("get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var picture = await _pictureBusiness.GetPicture(id);
            if (picture == null)
            {
                return null;
            }

            return File(picture.BinaryData, picture.MimeType);
        }
    }
}
