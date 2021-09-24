using HotChocolate;
using HotChocolate.Types;

namespace Module.Api.Product.Models
{
    public class AttributeRelationValueResultModel
    {
        [GraphQLType(typeof(LongType))]
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal PriceAdjustment { get; set; }
        public decimal PricePercentageAdjustment { get; set; }
        public int Quantity { get; set; }
        public int DisplayOrder { get; set; }
    }
}
