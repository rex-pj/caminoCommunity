using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Article.Api.Models
{
    public class ArticleIdFilterModel : BaseIdFilterModel<long>
    {
        public ArticleIdFilterModel() : base()
        {
        }
    }
}
