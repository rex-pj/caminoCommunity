using Microsoft.AspNetCore.Http;

namespace Module.Media.Api.Models
{
    public class ImageValidationModel
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
    }
}
