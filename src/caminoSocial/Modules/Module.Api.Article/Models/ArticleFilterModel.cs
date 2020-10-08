using Camino.Framework.Models;

namespace Module.Api.Article.Models
{
    public class ArticleFilterModel : BaseFilterModel
    {
        public ArticleFilterModel() : base()
        {

        }

        public string UserIdentityId { get; set; }
    }
}
