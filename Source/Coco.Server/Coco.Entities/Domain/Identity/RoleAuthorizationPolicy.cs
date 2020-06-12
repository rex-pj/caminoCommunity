using System;
using System.ComponentModel.DataAnnotations;

namespace Coco.Entities.Domain.Identity
{
    public class RoleAuthorizationPolicy
    {
        public long RoleId { get; set; }
        [Required]
        public short AuthorizationPolicyId { get; set; }
        [Required]
        public DateTime GrantedDate { get; set; }
        [Required]
        public long GrantedById { get; set; }
        [Required]
        public bool IsGranted { get; set; }
        public virtual Role Role { get; set; }
        public virtual User GrantedBy { get; set; }
        public virtual AuthorizationPolicy AuthorizationPolicy { get; set; }
    }
}
