using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;

namespace Module.Api.Article.Models
{
    public class ArticleCategorySelectFilterModel : BaseSelectFilterModel
    {
        [GraphQLType(typeof(IntType))]
        public int CurrentId { get; set; }
        [GraphQLType(typeof(BooleanType))]
        public bool IsParentOnly { get; set; }
    }
}
