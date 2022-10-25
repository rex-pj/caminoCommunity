using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Article.Api.Models
{
    public class ArticleFilterModel : BaseFilterModel
    {
        public ArticleFilterModel() : base()
        {
        }

        public long? Id { get; set; }
        public string UserIdentityId { get; set; }
    }
}
