using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Product.WebAdmin.Models
{
    public class ProductAttributeFilterModel : BaseFilterModel
    {
        public int? StatusId { get; set; }
    }
}
