using Camino.Framework.Models;
using System;

namespace Module.Web.FarmManagement.Models
{
    public class FarmFilterModel : BaseFilterModel
    {
        public string Content { get; set; }
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public long? FarmTypeId { get; set; }
    }
}
