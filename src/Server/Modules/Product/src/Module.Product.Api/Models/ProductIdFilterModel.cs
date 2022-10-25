using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Product.Api.Models
{
    public class ProductIdFilterModel : BaseIdFilterModel<long>
    {
        public ProductIdFilterModel() : base()
        {
        }
    }
}
