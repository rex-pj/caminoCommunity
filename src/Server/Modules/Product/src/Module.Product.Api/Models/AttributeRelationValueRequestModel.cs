namespace Module.Product.Api.Models
{
    public class AttributeRelationValueRequestModel
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public decimal PriceAdjustment { get; set; }
        public decimal PricePercentageAdjustment { get; set; }
        public int Quantity { get; set; }
        public int DisplayOrder { get; set; }
    }
}
