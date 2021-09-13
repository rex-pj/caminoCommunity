using Camino.Framework.Models;

namespace Module.Api.Product.Models
{
    public class ProductFilterModel : BaseFilterModel
    {
        public ProductFilterModel() : base()
        {
        }

        public long? Id { get; set; }
        public long? FarmId { get; set; }
        public string UserIdentityId { get; set; }
    }
}
