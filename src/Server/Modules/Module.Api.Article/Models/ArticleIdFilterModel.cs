using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Api.Article.Models
{
    public class ArticleIdFilterModel : BaseIdFilterModel<long>
    {
        public ArticleIdFilterModel() : base()
        {
        }
    }
}
