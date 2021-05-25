namespace Module.Web.ProductManagement.Models
{
    public class ProductAttributeRelationValueModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal PriceAdjustment { get; set; }
        public decimal PricePercentageAdjustment { get; set; }
        public int Quantity { get; set; }
        public int DisplayOrder { get; set; }
    }
}
