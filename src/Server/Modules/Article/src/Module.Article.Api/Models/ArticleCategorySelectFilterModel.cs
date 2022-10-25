using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Article.Api.Models
{
    public class ArticleCategorySelectFilterModel : BaseSelectFilterModel
    {
        public int? CurrentId { get; set; }
        public bool? IsParentOnly { get; set; }
    }
}
