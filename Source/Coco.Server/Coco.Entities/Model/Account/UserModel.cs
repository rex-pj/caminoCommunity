using System;

namespace Coco.Entities.Model.Account
{
    public class UserModel
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
        public bool IsActived { get; set; }
        public byte StatusId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string AuthenticatorToken { get; set; }
        public DateTime? Expiration { get; set; }
        public string SecurityStamp { get; set; }
    }
}
