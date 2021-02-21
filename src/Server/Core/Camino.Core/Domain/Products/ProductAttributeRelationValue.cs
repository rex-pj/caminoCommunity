namespace Camino.Core.Domain.Products
{
    public class ProductAttributeRelationValue
    {
		public long Id { get; set; }
		public string Name { get; set; }
		public long ProductAttributeRelationId { get; set; }
		public decimal PriceAdjustment { get; set; }
		public decimal PricePercentageAdjustment { get; set; }
		public int Quantity { get; set; }
		public int DisplayOrder { get; set; }
		public virtual ProductAttributeRelation ProductAttributeRelation { get; set; }
	}
}
