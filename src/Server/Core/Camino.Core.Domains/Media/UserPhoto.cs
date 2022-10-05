using System;
using System.ComponentModel.DataAnnotations;

namespace Camino.Core.Domains.Media
{
    public class UserPhoto
    {
        public long Id { get; set; }
        [Required]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public long CreatedById { get; set; }
        [Required]
        public byte[] FileData { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public int TypeId { get; set; }
        public virtual UserPhotoType UserPhotoType { get; set; }
    }
}
