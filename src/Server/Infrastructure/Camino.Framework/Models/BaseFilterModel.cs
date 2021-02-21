using HotChocolate;
using HotChocolate.Types;

namespace Camino.Framework.Models
{
    public class BaseFilterModel
    {
        public BaseFilterModel()
        {
            Page = 1;
            PageSize = 10;
        }

        [GraphQLType(typeof(LongType))]
        public int PageSize { get; set; }
        [GraphQLType(typeof(LongType))]
        public int Page { get; set; }
        public string Search { get; set; }
    }
}
