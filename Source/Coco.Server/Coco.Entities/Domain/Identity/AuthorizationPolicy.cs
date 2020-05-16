using System;
using System.Collections.Generic;

namespace Coco.Entities.Domain.Identity
{
    public class AuthorizationPolicy
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual ICollection<UserAuthorizationPolicy> AuthorizationPolicyUsers { get; set; }
        public virtual ICollection<RoleAuthorizationPolicy> AuthorizationPolicyRoles { get; set; }
    }
}
