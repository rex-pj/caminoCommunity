using Camino.Framework.Controllers;
using Camino.Service.Business.Media.Contracts;
using Camino.Service.Data.FileEntry;
using Camino.Service.FileStore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Module.Web.UploadManagement.Controllers
{
    public class ImageController : BaseAuthController
    {
        private readonly BaseFileStore _fileStore;
        private readonly IContentTypeProvider _contentTypeProvider;
        private readonly IPictureBusiness _pictureBusiness;

        public ImageController(IHttpContextAccessor httpContextAccessor, IContentTypeProvider contentTypeProvider,
            BaseFileStore fileStore, IPictureBusiness pictureBusiness)
            : base(httpContextAccessor)
        {
            _contentTypeProvider = contentTypeProvider;
            _fileStore = fileStore;
            _pictureBusiness = pictureBusiness;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UploadToBase64(IFormFile file)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        var fileBytes = memoryStream.ToArray();
                        var base64Url = Convert.ToBase64String(fileBytes);

                        return Json(new
                        {
                            name = file.FileName,
                            size = file.Length,
                            url = base64Url,
                            contentType = file.ContentType
                        });
                    }
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
                    var mediaFilePath = _fileStore.Combine("temp", path, file.FileName);
                    mediaFilePath = await _fileStore.CreateFileAsync(mediaFilePath, stream);

                    var mediaFile = await _fileStore.GetFileInfoAsync(mediaFilePath);
                    return Json(CreateFileResult(mediaFile));
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

        public object CreateFileResult(FileEntryInfo mediaFile)
        {
            _contentTypeProvider.TryGetContentType(mediaFile.Name, out var contentType);

            return new
            {
                name = mediaFile.Name,
                size = mediaFile.Length,
                folder = mediaFile.DirectoryPath,
                url = _fileStore.MapPathToPublicUrl(mediaFile.Path),
                mediaPath = mediaFile.Path,
                mime = contentType ?? "application/octet-stream"
            };
        }

        [HttpGet]
        [IgnoreAntiforgeryToken]
        public IActionResult GetPicture(long id)
        {
            var picture = _pictureBusiness.Find(id);
            var bynaryData = picture.BinaryData;

            return File(bynaryData, picture.MimeType);
        }
    }
}