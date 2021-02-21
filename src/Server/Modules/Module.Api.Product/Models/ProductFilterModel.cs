using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;

namespace Module.Api.Product.Models
{
    public class ProductFilterModel : BaseFilterModel
    {
        public ProductFilterModel() : base()
        {
            Page = 1;
            PageSize = 10;
        }

        [GraphQLType(typeof(LongType))]
        public long Id { get; set; }
        public long? FarmId { get; set; }
        public string UserIdentityId { get; set; }
    }
}
