using HotChocolate;
using HotChocolate.Types;

namespace Camino.Framework.Models
{
    public class BaseFilterModel
    {
        public BaseFilterModel()
        {
            Page = 1;
        }

        [GraphQLType(typeof(IntType))]
        public int Page { get; set; }
        [GraphQLType(typeof(IntType))]
        public int? PageSize { get; set; }
        public string Search { get; set; }
    }
}
