using Camino.Core.Domains.Authentication;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Identifiers;
using System.ComponentModel.DataAnnotations;

namespace Camino.Core.Domains.Users
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
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(255)]
        public string UserName { get; set; }
        [MaxLength(255)]
        public string Lastname { get; set; }
        [MaxLength(255)]
        public string Firstname { get; set; }
        [MaxLength(255)]
        public string DisplayName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public string SecurityStamp { get; set; }
        public bool IsEmailConfirmed { get; set; }

        // Additional information
        [Phone]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public string Description { get; set; }

        public DateTime? BirthDate { get; set; }
        public int? GenderId { get; set; }
        public short? CountryId { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Country Country { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }
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
