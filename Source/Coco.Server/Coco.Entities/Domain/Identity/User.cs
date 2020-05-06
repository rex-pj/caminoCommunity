using Coco.Entities.Base;
using Coco.Entities.Constant;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Domain.Work;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Identity
{
    [Table(nameof(User), Schema = TableSchemaConst.DBO)]
    public class User : BaseEntity
    {
        public User()
        {
            CreatedUsers = new HashSet<User>();
            UpdatedUsers = new HashSet<User>();
            UserCareers = new HashSet<UserCareer>();
            UserRoles = new HashSet<UserRole>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ForeignKey("UserInfo")]
        public long Id { get; set; }
        public string Email { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        [ForeignKey("CreatedBy")]
        public long? CreatedById { get; set; }
        
        [ForeignKey("UpdatedBy")]
        public long? UpdatedById { get; set; }
        public bool IsActived { get; set; }

        [ForeignKey("Status")]
        public byte StatusId { get; set; }
        public virtual bool IsEmailConfirmed { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual Status Status { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ICollection<User> CreatedUsers { get; set; }

        [ForeignKey("UpdatedById")]
        public virtual ICollection<User> UpdatedUsers { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<UserCareer> UserCareers { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
