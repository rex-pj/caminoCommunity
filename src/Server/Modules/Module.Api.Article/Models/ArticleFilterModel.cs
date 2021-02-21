using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;

namespace Module.Api.Article.Models
{
    public class ArticleFilterModel : BaseFilterModel
    {
        public ArticleFilterModel() : base()
        {
            Page = 1;
            PageSize = 10;
        }

        [GraphQLType(typeof(LongType))]
        public long Id { get; set; }
        public string UserIdentityId { get; set; }
    }
}
