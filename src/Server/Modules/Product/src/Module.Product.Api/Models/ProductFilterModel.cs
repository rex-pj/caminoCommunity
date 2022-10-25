using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Product.Api.Models
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
