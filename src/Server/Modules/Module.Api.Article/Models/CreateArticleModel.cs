using Camino.Framework.Models;

namespace Module.Api.Article.Models
{
    public class CreateArticleModel
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public PictureRequestModel Picture { get; set; }
        public int ArticleCategoryId { get; set; }
    }
}
