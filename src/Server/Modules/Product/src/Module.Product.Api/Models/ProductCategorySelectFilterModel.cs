using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Product.Api.Models
{
    public class ProductCategorySelectFilterModel : BaseSelectFilterModel
    {
        public bool? IsParentOnly { get; set; }
        public long[] CurrentIds { get; set; }
    }
}
