using System.Collections.Generic;

namespace Module.Product.WebAdmin.Models
{
    public class ProductAttributeRelationModel
    {
        public ProductAttributeRelationModel()
        {
            AttributeRelationValues = new List<ProductAttributeRelationValueModel>();
        }

        public long Id { get; set; }
        public int AttributeId { get; set; }
        public string Name { get; set; }
        public int ControlTypeId { get; set; }
        public string ControlTypeName { get; set; }
        public int DisplayOrder { get; set; }
        public string TextPrompt { get; set; }
        public bool IsRequired { get; set; }

        public IEnumerable<ProductAttributeRelationValueModel> AttributeRelationValues { get; set; }
    }
}
