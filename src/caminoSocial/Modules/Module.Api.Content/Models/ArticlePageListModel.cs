using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Api.Content.Models
{
    public class ArticlePageListModel : PageListModel<ArticleModel>
    {
        public ArticlePageListModel(IEnumerable<ArticleModel> collections) : base(collections)
        {
        }
    }
}
