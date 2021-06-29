using System;

namespace Camino.Shared.Requests.Identifiers
{
    public class UserModifyRequest
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long CreatedById { get; set; }
        public long UpdatedById { get; set; }
        public byte? GenderId { get; set; }
        public short? CountryId { get; set; }
        public int StatusId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
