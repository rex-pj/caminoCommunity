using Camino.Infrastructure.AspNetCore.Models;
using System;

namespace Module.Product.WebAdmin.Models
{
    public class ProductPictureFilterModel : BaseFilterModel
    {
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public string MimeType { get; set; }
    }
}
