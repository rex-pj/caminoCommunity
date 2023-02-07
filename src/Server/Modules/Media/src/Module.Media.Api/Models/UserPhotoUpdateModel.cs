using System.ComponentModel.DataAnnotations;

namespace Module.Media.Api.Models
{
    public class UserPhotoUpdateModel
    {
        [Required]
        public double XAxis { get; set; }
        [Required]
        public double YAxis { get; set; }
        [Required]
        public double Width { get; set; }
        [Required]
        public double Height { get; set; }
        [Required]
        public double Scale { get; set; }
        [Required]
        public string FileName { get; set; }
    }
}
