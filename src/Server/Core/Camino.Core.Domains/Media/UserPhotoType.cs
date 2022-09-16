using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Camino.Core.Domains.Media
{
    public class UserPhotoType
    {
        public UserPhotoType()
        {
            UserPhotos = new HashSet<UserPhoto>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserPhoto> UserPhotos { get; set; }
    }
}
