using Coco.Entities.Constant;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Identity
{
    [Table(nameof(UserAuthorizationPolicy), Schema = TableSchemaConst.DBO)]
    public class UserAuthorizationPolicy
    {
        public long UserId { get; set; }

        public short AuthorizationPolicyId { get; set; }
        public DateTime GrantedDate { get; set; }
        public long GrantedById { get; set; }
        public bool IsGranted { get; set; }
        public virtual User User { get; set; }
        public virtual User GrantedBy { get; set; }
        public virtual AuthorizationPolicy AuthorizationPolicy { get; set; }
    }
}
