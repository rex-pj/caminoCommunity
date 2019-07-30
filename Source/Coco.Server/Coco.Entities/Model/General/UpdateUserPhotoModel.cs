using Coco.Entities.Enums;

namespace Coco.Entities.Model.General
{
    public class UpdateUserPhotoModel
    {
        public string PhotoUrl { get; set; }
        public double XAxis { get; set; }
        public double YAxis { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string ContentType { get; set; }
        public bool CanEdit { get; set; }
        public string FileName { get; set; }
        public string UserPhotoCode { get; set; }
        public UserPhotoTypeEnum UserPhotoType { get; set; }
    }
}
