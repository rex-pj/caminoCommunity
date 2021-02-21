using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;

namespace Module.Api.Article.Models
{
    public class ArticleCategorySelectFilterModel : BaseSelectFilterModel
    {
        [GraphQLType(typeof(LongType))]
        public long CurrentId { get; set; }
        [GraphQLType(typeof(BooleanType))]
        public bool IsParentOnly { get; set; }
    }
}
