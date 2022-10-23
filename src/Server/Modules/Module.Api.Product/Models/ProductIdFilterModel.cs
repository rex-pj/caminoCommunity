using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Api.Product.Models
{
    public class ProductIdFilterModel : BaseIdFilterModel<long>
    {
        public ProductIdFilterModel() : base()
        {
        }
    }
}
