using Camino.Framework.Models;

namespace Module.Api.Product.Models
{
    public class ProductFilterModel : BaseFilterModel
    {
        public ProductFilterModel() : base()
        {
            Page = 1;
            PageSize = 10;
        }

        public long Id { get; set; }
        public long? FarmId { get; set; }
        public string UserIdentityId { get; set; }
    }
}
