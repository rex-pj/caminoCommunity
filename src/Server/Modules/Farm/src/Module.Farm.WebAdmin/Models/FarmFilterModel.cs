using Camino.Infrastructure.AspNetCore.Models;
using System;

namespace Module.Farm.WebAdmin.Models
{
    public class FarmFilterModel : BaseFilterModel
    {
        public string Content { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public long? FarmTypeId { get; set; }
        public int? StatusId { get; set; }
    }
}
