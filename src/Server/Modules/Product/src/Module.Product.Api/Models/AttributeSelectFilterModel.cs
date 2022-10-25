using Camino.Infrastructure.AspNetCore.Models;
using System.Collections.Generic;

namespace Module.Product.Api.Models
{
    public class AttributeSelectFilterModel : BaseSelectFilterModel
    {
        public IEnumerable<int> ExcludedIds { get; set; }
    }
}
