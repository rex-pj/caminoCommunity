using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Camino.Shared.Enums;
using Camino.Core.Contracts.Services.Users;
using Camino.Core.Contracts.Services.Media;

namespace Module.Api.Media.Controllers
{
    [Route("file-api/[controller]")]
    public class PhotoController : Controller
    {
        private readonly IUserPhotoService _userPhotoService;
        private readonly IPictureService _pictureService;

        public PhotoController(IUserPhotoService userPhotoService, IPictureService pictureService)
        {
            _userPhotoService = userPhotoService;
            _pictureService = pictureService;
        }

        [HttpGet]
        [Route("avatar/{code}")]
        public async Task<IActionResult> GetAvatar(string code)
        {
            var avatar = await _userPhotoService.GetUserPhotoByCodeAsync(code, UserPhotoKind.Avatar);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }

        [HttpGet]
        [Route("cover/{code}")]
        public async Task<IActionResult> GetCover(string code)
        {
            var avatar = await _userPhotoService.GetUserPhotoByCodeAsync(code, UserPhotoKind.Cover);
            var bytes = Convert.FromBase64String(avatar.ImageData);

            return File(bytes, "image/jpeg");
        }

        [HttpGet]
        [IgnoreAntiforgeryToken]
        [Route("get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var picture = await _pictureService.FindPictureAsync(id);
            if (picture == null)
            {
                return null;
            }

            return File(picture.BinaryData, picture.ContentType);
        }
    }
}
