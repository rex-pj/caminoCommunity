using Coco.Core.Entities.Enums;

namespace Coco.Core.Dtos.General
{
    public class UserPhotoUpdateDto
    {
        public string PhotoUrl { get; set; }
        public double XAxis { get; set; }
        public double YAxis { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Scale { get; set; }
        public string ContentType { get; set; }
        public bool CanEdit { get; set; }
        public string FileName { get; set; }
        public string UserPhotoCode { get; set; }
        public UserPhotoType UserPhotoType { get; set; }
    }
}
