using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Camino.Core.Domain.Media
{
    public class UserPhotoType
    {
        public UserPhotoType()
        {
            UserPhotos = new HashSet<UserPhoto>();
        }

        public byte Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserPhoto> UserPhotos { get; set; }
    }
}
