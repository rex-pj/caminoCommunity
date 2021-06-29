using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Camino.Core.Domain.Identifiers
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
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Lastname { get; set; }
        [Required]
        [MaxLength(255)]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(255)]
        public string DisplayName { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        [Required]
        public DateTimeOffset UpdatedDate { get; set; }
        [Required]
        public long CreatedById { get; set; }
        [Required]
        public long UpdatedById { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public string SecurityStamp { get; set; }
        public bool IsEmailConfirmed { get; set; }
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
        public virtual ICollection<UserRole> GrantedUserRoles { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
    }
}
