using Camino.Framework.Models;

namespace Module.Api.Product.Models
{
    public class ProductFilterModel : BaseFilterModel
    {
        public ProductFilterModel() : base()
        {

        }

        public string UserIdentityId { get; set; }
    }
}
