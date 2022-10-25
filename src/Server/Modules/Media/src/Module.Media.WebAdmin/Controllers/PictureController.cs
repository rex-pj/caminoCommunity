using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Media;
using Camino.Infrastructure.Identity.Attributes;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Infrastructure.Files.Contracts;
using Camino.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Module.Media.WebAdmin.Controllers
{
    [Route("pictures")]
    public class PictureController : BaseAuthController
    {
        private readonly IMediaFileStore _mediaFileStore;
        private readonly IFileStore _fileStore;
        private readonly IPictureAppService _pictureappService;

        public PictureController(IHttpContextAccessor httpContextAccessor,
            IMediaFileStore mediaFileStore, IFileStore fileStore, IPictureAppService pictureAppService)
            : base(httpContextAccessor)
        {
            _mediaFileStore = mediaFileStore;
            _pictureappService = pictureAppService;
            _fileStore = fileStore;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        [ApplicationAuthorize(AuthorizePolicies.CanCreatePicture)]
        public async Task<IActionResult> ConverToBase64(IFormFile file)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var base64Url = await _fileStore.CreateFileAsync(stream);
                    return Json(new
                    {
                        name = file.FileName,
                        size = file.Length,
                        url = base64Url,
                        contentType = file.ContentType
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    name = file.FileName,
                    size = file.Length,
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        [ApplicationAuthorize(AuthorizePolicies.CanCreatePicture)]
        public async Task<IActionResult> TemporaryUpload(string path, IFormFile file)
        {
            if (path == null)
            {
                path = "";
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var mediaFilePath = _mediaFileStore.Combine("temp", path, file.FileName);
                    mediaFilePath = await _mediaFileStore.CreateFileAsync(mediaFilePath, stream);

                    var mediaFile = await _mediaFileStore.GetFileInfoAsync(mediaFilePath);

                    return Json(new
                    {
                        name = mediaFile.Name,
                        size = mediaFile.Length,
                        folder = mediaFile.DirectoryPath,
                        url = _mediaFileStore.MapPathToPublicUrl(mediaFile.Path),
                        mediaPath = mediaFile.Path,
                        mime = file.ContentType
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    name = file.FileName,
                    size = file.Length,
                    folder = path,
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        [IgnoreAntiforgeryToken]
        [ApplicationAuthorize(AuthorizePolicies.CanReadPicture)]
        [Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var exist = await _pictureappService.FindAsync(new IdRequestFilter<long>
            {
                Id = id,
                CanGetDeleted = true,
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