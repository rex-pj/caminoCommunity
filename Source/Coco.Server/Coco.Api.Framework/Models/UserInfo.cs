using System;

namespace Coco.Api.Framework.Models
{
    public class UserInfo
    {
        public string UserHashedId { get; set; }
        public string Email { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte? GenderId { get; set; }
        public string GenderLabel { get; set; }
        public bool IsActived { get; set; }
        public byte StatusId { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }
}
