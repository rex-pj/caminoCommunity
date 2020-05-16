using Coco.Entities.Constant;
using Coco.Entities.Domain.Auth;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Identity
{
    [Table(nameof(RoleAuthorizationPolicy), Schema = TableSchemaConst.DBO)]
    public class RoleAuthorizationPolicy
    {
        public byte RoleId { get; set; }

        public short AuthorizationPolicyId { get; set; }
        public DateTime GrantedDate { get; set; }
        public long GrantedById { get; set; }
        public bool IsGranted { get; set; }
        public virtual Role Role { get; set; }
        public virtual User GrantedBy { get; set; }
        public virtual AuthorizationPolicy AuthorizationPolicy { get; set; }
    }
}
