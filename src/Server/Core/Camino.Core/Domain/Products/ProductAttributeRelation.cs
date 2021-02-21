using System.Collections.Generic;

namespace Camino.Core.Domain.Products
{
    public class ProductAttributeRelation
    {
		public long Id { get; set; }
		public int ProductAttributeId { get; set; }
		public long ProductId { get; set; }
		public string TextPrompt { get; set; }
		public bool IsRequired { get; set; }
		public int AttributeControlTypeId { get; set; }
		public int DisplayOrder { get; set; }
		public virtual ProductAttribute ProductAttribute { get; set; }
		public virtual ICollection<ProductAttributeRelationValue> ProductAttributeRelationValues { get; set; }
	}
}
