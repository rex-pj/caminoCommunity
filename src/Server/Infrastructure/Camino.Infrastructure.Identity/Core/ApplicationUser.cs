using Microsoft.AspNetCore.Identity;

namespace Camino.Infrastructure.Identity.Core
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public long CreatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public byte? GenderId { get; set; }
        public string GenderLabel { get; set; }

        // Extend
        public short? CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public int StatusId { get; set; }
        public string UserIdentityId { get; set; }
        public string StatusLabel { get; set; }
        public string AuthenticationToken { get; set; }
    }
}
