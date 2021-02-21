using Camino.Core.Constants;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Core.Contracts.Services.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Camino.Core.Contracts.FileStore;

namespace Module.Web.UploadManagement.Controllers
{
    public class PictureController : BaseAuthController
    {
        private readonly IMediaFileStore _mediaFileStore;
        private readonly IFileStore _fileStore;
        private readonly IPictureService _pictureService;

        public PictureController(IHttpContextAccessor httpContextAccessor,
            IMediaFileStore mediaFileStore, IFileStore fileStore, IPictureService pictureService)
            : base(httpContextAccessor)
        {
            _mediaFileStore = mediaFileStore;
            _pictureService = pictureService;
            _fileStore = fileStore;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreatePicture)]
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
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreatePicture)]
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
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadPicture)]
        public async Task<IActionResult> Get(long id)
        {
            var picture = await _pictureService.FindPictureAsync(id);
            return File(picture.BinaryData, picture.ContentType);
        }
    }
}