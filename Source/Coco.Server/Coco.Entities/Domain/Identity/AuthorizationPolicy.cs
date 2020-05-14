using Coco.Entities.Base;
using Coco.Entities.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Identity
{
    [Table(nameof(AuthorizationPolicy), Schema = TableSchemaConst.DBO)]
    public class AuthorizationPolicy : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("CreatedBy")]
        public long CreatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        [ForeignKey("UpdatedBy")]
        public long UpdatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual ICollection<UserAuthorizationPolicy> AuthorizationPolicyUsers { get; set; }
        public virtual ICollection<RoleAuthorizationPolicy> AuthorizationPolicyRoles { get; set; }
    }
}
