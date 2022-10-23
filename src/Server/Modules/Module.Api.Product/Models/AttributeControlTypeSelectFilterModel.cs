using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Api.Product.Models
{
    public class AttributeControlTypeSelectFilterModel : BaseSelectFilterModel
    {
        public int? CurrentId { get; set; }
    }
}
