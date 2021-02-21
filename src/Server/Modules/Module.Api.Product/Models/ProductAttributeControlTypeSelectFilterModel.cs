using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;

namespace Module.Api.Product.Models
{
    public class ProductAttributeControlTypeSelectFilterModel : BaseSelectFilterModel
    {
        [GraphQLType(typeof(LongType))]
        public int CurrentId { get; set; }
    }
}
