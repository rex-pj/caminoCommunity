using Camino.Framework.Models;
using System;

namespace Module.Web.FarmManagement.Models
{
    public class FarmTypeFilterModel : BaseFilterModel
    {
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int? StatusId { get; set; }
    }
}
