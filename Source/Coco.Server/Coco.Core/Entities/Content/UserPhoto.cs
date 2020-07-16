using System;
using System.ComponentModel.DataAnnotations;

namespace Coco.Core.Entities.Content
{
    public class UserPhoto
    {
        public long Id { get; set; }
        [Required]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public long CreatedById { get; set; }
        [Required]
        public string ImageData { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public byte TypeId { get; set; }
        public virtual UserPhotoType Type { get; set; }
    }
}
