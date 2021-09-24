using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Api.Product.Models
{
    public class AttributeSelectFilterModel : BaseSelectFilterModel
    {
        public IEnumerable<int> ExcludedIds { get; set; }
    }
}
