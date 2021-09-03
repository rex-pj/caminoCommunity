using System;

namespace Camino.Framework.Models
{
    public class UserInfoModel
    {
        public string UserIdentityId { get; set; }
        public string Email { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public int? GenderId { get; set; }
        public string GenderLabel { get; set; }
        public int StatusId { get; set; }
        public string StatusLabel { get; set; }
        public short? CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string AvatarCode { get; set; }
        public bool CanEdit { get; set; }
    }
}
