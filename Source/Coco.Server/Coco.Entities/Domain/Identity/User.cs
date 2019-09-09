using Coco.Entities.Base;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Domain.Work;
using System;
using System.Collections.Generic;

namespace Coco.Entities.Domain.Identity
{
    public class User : BaseEntity
    {
        public User()
        {
            this.CreatedUsers = new HashSet<User>();
            this.UpdatedUsers = new HashSet<User>();
            this.UserCareers = new HashSet<UserCareer>();
            this.UserRoles = new HashSet<UserRole>();
        }
        
        public long Id { get; set; }
        public string Email { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string AuthenticatorToken { get; set; }
        public string IdentityStamp { get; set; }
        public string SecurityStamp { get; set; }
        public DateTime? Expiration { get; set; }
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
        public virtual ICollection<UserCareer> UserCareers { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
