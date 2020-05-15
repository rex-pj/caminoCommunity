using Coco.Entities.Base;
using Coco.Entities.Constant;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Dbo
{
    [Table(nameof(UserPhotoType), Schema = TableSchemaConst.DBO)]
    public class UserPhotoType : BaseEntity
    {
        public UserPhotoType()
        {
            UserPhotos = new HashSet<UserPhoto>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserPhoto> UserPhotos { get; set; }
    }
}
