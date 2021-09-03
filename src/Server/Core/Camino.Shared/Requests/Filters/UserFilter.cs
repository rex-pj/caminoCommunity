using System;

namespace Camino.Shared.Requests.Filters
{
    public class UserFilter : BaseFilter
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTimeOffset? BirthDateFrom { get; set; }
        public DateTimeOffset? BirthDateTo { get; set; }
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public byte? GenderId { get; set; }
        public short? CountryId { get; set; }
        public int? StatusId { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public long? ExclusiveUserById { get; set; }
        public bool CanGetDeleted { get; set; }
        public bool CanGetInactived { get; set; }
    }
}
