using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Product.Api.Models
{
    public class AttributeControlTypeSelectFilterModel : BaseSelectFilterModel
    {
        public int? CurrentId { get; set; }
    }
}
