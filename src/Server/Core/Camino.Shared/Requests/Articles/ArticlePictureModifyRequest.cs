using Camino.Shared.Requests.Media;
using System;

namespace Camino.Shared.Requests.Articles
{
    public class ArticlePictureModifyRequest
    {
        public ArticlePictureModifyRequest()
        {
            Picture = new PictureRequest();
        }

        public long CreatedById { get; set; }
        public long UpdatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        public long ArticleId { get; set; }

        public PictureRequest Picture { get; set; }
    }
}
