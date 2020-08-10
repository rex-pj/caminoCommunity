using System;

namespace Camino.Business.Dtos.General
{
    public class UserFilterDto : BaseFilterDto
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDateFrom { get; set; }
        public DateTime? BirthDateTo { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public DateTime? UpdatedDateFrom { get; set; }
        public DateTime? UpdatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public byte? GenderId { get; set; }
        public short? CountryId { get; set; }
        public bool? IsActived { get; set; }
        public int? StatusId { get; set; }
    }
}
