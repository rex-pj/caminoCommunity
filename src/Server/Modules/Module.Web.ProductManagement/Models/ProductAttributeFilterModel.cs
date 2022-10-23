using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Web.ProductManagement.Models
{
    public class ProductAttributeFilterModel : BaseFilterModel
    {
        public int? StatusId { get; set; }
    }
}
