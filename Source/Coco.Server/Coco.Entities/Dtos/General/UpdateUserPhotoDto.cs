using Coco.Entities.Enums;

namespace Coco.Entities.Dtos.General
{
    public interface IUserPhotoUpdateDto
    {
        string PhotoUrl { get; set; }
        double XAxis { get; set; }
        double YAxis { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        double Scale { get; set; }
        string ContentType { get; set; }
        bool CanEdit { get; set; }
        string FileName { get; set; }
        string UserPhotoCode { get; set; }
        UserPhotoTypeEnum UserPhotoType { get; set; }
    }

    public class UserPhotoUpdateDto : IUserPhotoUpdateDto
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
        public UserPhotoTypeEnum UserPhotoType { get; set; }
    }
}
