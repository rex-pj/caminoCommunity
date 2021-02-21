using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;

namespace Module.Api.Product.Models
{
    public class ProductCategorySelectFilterModel : BaseSelectFilterModel
    {
        [GraphQLType(typeof(BooleanType))]
        public bool IsParentOnly { get; set; }
        public long[] CurrentIds { get; set; }
    }
}
