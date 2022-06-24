using Camino.Framework.Models;

namespace Module.Api.Product.Models
{
    public class ProductCategorySelectFilterModel : BaseSelectFilterModel
    {
        public bool? IsParentOnly { get; set; }
        public long[] CurrentIds { get; set; }
    }
}
