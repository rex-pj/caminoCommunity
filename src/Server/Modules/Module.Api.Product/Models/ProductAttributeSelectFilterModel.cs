using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Api.Product.Models
{
    public class ProductAttributeSelectFilterModel : BaseSelectFilterModel
    {
        public IEnumerable<int> ExcludedIds { get; set; }
    }
}
