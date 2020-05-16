using Coco.Entities.Domain.Auth;
using System;
using System.Collections.Generic;

namespace Coco.Entities.Domain.Identity
{
    public class User
    {
        public User()
        {
            CreatedUsers = new HashSet<User>();
            UpdatedUsers = new HashSet<User>();
            UserRoles = new HashSet<UserRole>();
            UserAuthorizationPolicies = new HashSet<UserAuthorizationPolicy>();
            GrantedUserAuthorizationPolicies = new HashSet<UserAuthorizationPolicy>();
            GrantedRoleAuthorizationPolicies = new HashSet<RoleAuthorizationPolicy>();
        }
        
        public long Id { get; set; }
        public string Email { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        public long? CreatedById { get; set; }
        
        public long? UpdatedById { get; set; }
        public bool IsActived { get; set; }

        public byte StatusId { get; set; }
        public virtual bool IsEmailConfirmed { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual Status Status { get; set; }

        public virtual ICollection<User> CreatedUsers { get; set; }

        public virtual ICollection<User> UpdatedUsers { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Role> CreatedRoles { get; set; }
        public virtual ICollection<Role> UpdatedRoles { get; set; }
        public virtual ICollection<AuthorizationPolicy> CreatedAuthorizationPolicies { get; set; }
        public virtual ICollection<AuthorizationPolicy> UpdatedAuthorizationPolicies { get; set; }
        public virtual ICollection<UserAuthorizationPolicy> UserAuthorizationPolicies { get; set; }
        public virtual ICollection<UserAuthorizationPolicy> GrantedUserAuthorizationPolicies { get; set; }
        public virtual ICollection<RoleAuthorizationPolicy> GrantedRoleAuthorizationPolicies { get; set; }
    }
}
