using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Camino.Framework.Controllers;
using Camino.Application.Contracts.AppServices.Users;
using Camino.Application.Contracts.AppServices.Media;
using Camino.Shared.Enums;
using Camino.Application.Contracts;

namespace Module.Api.Media.Controllers
{
    [Route("pictures")]
    public class PictureController : BaseController
    {
        private readonly IUserPhotoAppService _userPhotoAppService;
        private readonly IPictureAppService _pictureAppService;

        public PictureController(IUserPhotoAppService userPhotoAppService, IPictureAppService pictureAppService)
        {
            _userPhotoAppService = userPhotoAppService;
            _pictureAppService = pictureAppService;
        }

        [HttpGet]
        [Route("avatars/{code}")]
        public async Task<IActionResult> GetAvatar(string code)
        {
            var avatar = await _userPhotoAppService.GetByCodeAsync(code, UserPictureTypes.Avatar);
            return File(avatar.BinaryData, "image/jpeg");
        }

        [HttpGet]
        [Route("covers/{code}")]
        public async Task<IActionResult> GetCover(string code)
        {
            var cover = await _userPhotoAppService.GetByCodeAsync(code, UserPictureTypes.Cover);
            return File(cover.BinaryData, "image/jpeg");
        }

        [HttpGet]
        [IgnoreAntiforgeryToken]
        [Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var exist = await _pictureAppService.FindAsync(new IdRequestFilter<long>
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
