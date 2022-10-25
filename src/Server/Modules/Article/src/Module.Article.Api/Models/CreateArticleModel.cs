using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Article.Api.Models
{
    public class CreateArticleModel
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public PictureRequestModel Picture { get; set; }
        public int ArticleCategoryId { get; set; }
    }
}
