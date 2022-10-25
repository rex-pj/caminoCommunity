using Camino.Infrastructure.AspNetCore.Models;
using System.Collections.Generic;

namespace Module.Article.Api.Models
{
    public class ArticlePageListModel : PageListModel<ArticleModel>
    {
        public ArticlePageListModel(IEnumerable<ArticleModel> collections) : base(collections)
        {
        }
    }
}
