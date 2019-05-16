using Coco.Api.Framework.AccountIdentity.Entities;
using System;

namespace Coco.Api.Framework.Models
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public long? CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? UpdatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte? GenderId { get; set; }
        public int? CountryId { get; set; }
        public bool IsActived { get; set; }
        public byte StatusId { get; set; }
    }
}
