using Camino.Framework.Models;
using System;

namespace Module.Web.AuthenticationManagement.Models
{
    public class UserFilterModel : BaseFilterModel
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTimeOffset? BirthDateFrom { get; set; }
        public DateTimeOffset? BirthDateTo { get; set; }
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public byte GenderId { get; set; }
        public short? CountryId { get; set; }
        public bool? IsActived { get; set; }
        public int? StatusId { get; set; }
    }
}
