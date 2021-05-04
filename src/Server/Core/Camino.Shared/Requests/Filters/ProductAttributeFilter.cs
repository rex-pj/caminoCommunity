using System.Collections.Generic;

namespace Camino.Shared.Requests.Filters
{
    public class ProductAttributeFilter : BaseFilter
    {
        public ProductAttributeFilter()
        {
            ExcludedIds = new List<int>();
            PageSize = 20;
            Page = 1;
        }

        public IEnumerable<int> ExcludedIds { get; set; }
    }
}
