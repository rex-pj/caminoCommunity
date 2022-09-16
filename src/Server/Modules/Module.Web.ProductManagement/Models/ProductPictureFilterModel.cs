using Camino.Framework.Models;
using System;

namespace Module.Web.ProductManagement.Models
{
    public class ProductPictureFilterModel : BaseFilterModel
    {
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public string MimeType { get; set; }
    }
}
