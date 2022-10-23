using Camino.Infrastructure.AspNetCore.Models;
using HotChocolate;
using HotChocolate.Types;

namespace Module.Api.Article.Models
{
    public class UpdateArticleModel
    {
        [GraphQLType(typeof(LongType))]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public PictureRequestModel Picture { get; set; }
        public int ArticleCategoryId { get; set; }
    }
}
