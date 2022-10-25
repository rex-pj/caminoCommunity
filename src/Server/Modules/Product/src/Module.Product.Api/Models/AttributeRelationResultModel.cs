using System.Collections.Generic;

namespace Module.Product.Api.Models
{
    public class AttributeRelationResultModel
    {
        public AttributeRelationResultModel()
        {
            AttributeRelationValues = new List<AttributeRelationValueResultModel>();
        }

        public long Id { get; set; }
        public int AttributeId { get; set; }
        public string Name { get; set; }
        public int ControlTypeId { get; set; }
        public string ControlTypeName { get; set; }
        public int DisplayOrder { get; set; }
        public string TextPrompt { get; set; }
        public bool IsRequired { get; set; }

        public IEnumerable<AttributeRelationValueResultModel> AttributeRelationValues { get; set; }
    }
}
