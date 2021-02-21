using Camino.Shared.Requests.Media;
using System;

namespace Camino.Shared.Requests.Articles
{
    public class ArticleModifyRequest
    {
        public ArticleModifyRequest()
        {
            Picture = new PictureRequest();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public long UpdatedById { get; set; }
        public long CreatedById { get; set; }
        public int ArticleCategoryId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public PictureRequest Picture { get; set; }
    }
}
