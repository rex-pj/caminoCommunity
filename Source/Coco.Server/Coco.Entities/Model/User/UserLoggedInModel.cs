using System;

namespace Coco.Entities.Model.User
{
    public class UserLoggedInModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
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
        public string AuthenticationToken { get; set; }
        public DateTime? Expiration { get; set; }
        public string AvatarUrl { get; set; }
        public string CoverPhotoUrl { get; set; }
    }
}
