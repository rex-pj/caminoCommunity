using System.Collections.Generic;

namespace Camino.Shared.Requests.Products
{
    public class ProductAttributeRelationRequest
    {
        public ProductAttributeRelationRequest()
        {
            AttributeRelationValues = new List<ProductAttributeRelationValueRequest>();
        }

        public long Id { get; set; }
        public int ProductAttributeId { get; set; }
        public long ProductId { get; set; }
        public int ControlTypeId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsRequired { get; set; }
        public string TextPrompt { get; set; }

        public IEnumerable<ProductAttributeRelationValueRequest> AttributeRelationValues { get; set; }
    }
}
