using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Camino.Shared.Enums;
using Camino.Core.Contracts.Services.Users;
using Camino.Core.Contracts.Services.Media;
using Camino.Shared.Requests.Filters;
using Camino.Framework.Controllers;

namespace Module.Api.Media.Controllers
{
    [Route("pictures")]
    public class PictureController : BaseController
    {
        private readonly IUserPhotoService _userPhotoService;
        private readonly IPictureService _pictureService;

        public PictureController(IUserPhotoService userPhotoService, IPictureService pictureService)
        {
            _userPhotoService = userPhotoService;
            _pictureService = pictureService;
        }

        [HttpGet]
        [Route("avatars/{code}")]
        public async Task<IActionResult> GetAvatar(string code)
        {
            var avatar = await _userPhotoService.GetUserPhotoByCodeAsync(code, UserPictureType.Avatar);
            return File(avatar.BinaryData, "image/jpeg");
        }

        [HttpGet]
        [Route("covers/{code}")]
        public async Task<IActionResult> GetCover(string code)
        {
            var cover = await _userPhotoService.GetUserPhotoByCodeAsync(code, UserPictureType.Cover);
            return File(cover.BinaryData, "image/jpeg");
        }

        [HttpGet]
        [IgnoreAntiforgeryToken]
        [Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var exist = await _pictureService.FindAsync(new IdRequestFilter<long>
            {
                Id = id,
                CanGetInactived = true
            });

            if (exist == null)
            {
                return null;
            }

            return File(exist.BinaryData, exist.ContentType);
        }
    }
}
