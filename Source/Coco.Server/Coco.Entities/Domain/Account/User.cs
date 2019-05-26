using Coco.Entities.Base;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Domain.Farm;
using Coco.Entities.Domain.Work;
using System;
using System.Collections.Generic;

namespace Coco.Entities.Domain.Account
{
    public class User : BaseEntity
    {
        public User()
        {
            this.CreatedUserInfos = new HashSet<UserInfo>();
            this.UpdatedUserInfos = new HashSet<UserInfo>();
            this.UserCareers = new HashSet<UserCareer>();
            this.UserRoles = new HashSet<UserRole>();
        }
        
        public long Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string AuthenticatorToken { get; set; }
        public string SecurityStamp { get; set; }
        public DateTime? Expiration { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual ICollection<UserInfo> CreatedUserInfos { get; set; }
        public virtual ICollection<UserInfo> UpdatedUserInfos { get; set; }

        public virtual ICollection<UserCareer> UserCareers { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
