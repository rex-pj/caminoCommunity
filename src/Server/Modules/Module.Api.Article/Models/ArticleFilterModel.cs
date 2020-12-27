using Camino.Framework.Models;

namespace Module.Api.Article.Models
{
    public class ArticleFilterModel : BaseFilterModel
    {
        public ArticleFilterModel() : base()
        {
            Page = 1;
            PageSize = 10;
        }

        public long Id { get; set; }
        public string UserIdentityId { get; set; }
    }
}
