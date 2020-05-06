using Coco.Entities.Base;
using Coco.Entities.Constant;
using Coco.Entities.Domain.Dbo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Identity
{
    [Table(nameof(UserInfo), Schema = TableSchemaConst.DBO)]
    public class UserInfo : BaseEntity
    {
        public UserInfo()
        {
            this.UserPhotos = new HashSet<UserPhoto>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ForeignKey("User")]
        public long Id { get; set; }
        
        [Phone]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        
        [Required]
        public DateTime? BirthDate { get; set; }
        [ForeignKey("Gender")]
        public byte? GenderId { get; set; }
        [ForeignKey("Country")]
        public short? CountryId { get; set; }
        public string AvatarUrl { get; set; }
        public string CoverPhotoUrl { get; set; }
        public virtual User User { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Country Country { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<UserPhoto> UserPhotos { get; set; }
    }
}
