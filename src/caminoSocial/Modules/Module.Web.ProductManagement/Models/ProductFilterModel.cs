using Camino.Framework.Models;
using System;

namespace Module.Web.ProductManagement.Models
{
    public class ProductFilterModel : BaseFilterModel
    {
        public string Content { get; set; }
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public long? CategoryId { get; set; }
    }
}
