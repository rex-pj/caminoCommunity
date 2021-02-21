using System.Collections.Generic;

namespace Camino.Shared.Results.Products
{
    public class ProductAttributeRelationResult
    {
        public long Id { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string TextPrompt { get; set; }
        public bool IsRequired { get; set; }
        public int AttributeControlTypeId { get; set; }
        public int DisplayOrder { get; set; }

        public IEnumerable<ProductAttributeRelationValueResult> AttributeRelationValues { get; set; }
    }
}
