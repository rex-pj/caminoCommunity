using Camino.Framework.Models;
using System;

namespace Module.Web.AuthenticationManagement.Models
{
    public class UserModel : BaseModel
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public byte? GenderId { get; set; }
        public short? CountryId { get; set; }
        public int StatusId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string GenderLabel { get; set; }
        public string StatusLabel { get; set; }
    }
}
