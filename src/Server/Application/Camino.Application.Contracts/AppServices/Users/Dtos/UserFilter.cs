using System;

namespace Camino.Application.Contracts.AppServices.Users.Dtos
{
    public class UserFilter : BaseFilter
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDateFrom { get; set; }
        public DateTime? BirthDateTo { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int? GenderId { get; set; }
        public short? CountryId { get; set; }
        public int? StatusId { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public long? ExclusiveUserById { get; set; }
        public bool CanGetDeleted { get; set; }
        public bool CanGetInactived { get; set; }
    }
}
