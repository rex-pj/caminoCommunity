using Microsoft.AspNetCore.Http;

namespace Module.Api.Media.Models
{
    public class UserPhotoUpdateModel
    {
        public double XAxis { get; set; }
        public double YAxis { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Scale { get; set; }
        public string FileName { get; set; }

        public IFormFile File { get; set; }
    }
}
