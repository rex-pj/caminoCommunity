using Coco.Entities.Constant;
using Coco.Entities.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Auth
{
    [Table(nameof(Role), Schema = TableSchemaConst.DBO)]
    public class Role
    {
        public Role()
        {
            this.UserRoles = new HashSet<UserRole>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleAuthorizationPolicy> RoleAuthorizationPolicies { get; set; }
    }
}
