using Camino.Application.Contracts.AppServices.Media.Dtos;

namespace Camino.Application.Contracts.AppServices.Articles.Dtos
{
    public class ArticlePictureModifyRequest
    {
        public ArticlePictureModifyRequest()
        {
            Picture = new PictureRequest();
        }

        public long CreatedById { get; set; }
        public long UpdatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public long ArticleId { get; set; }

        public PictureRequest Picture { get; set; }
    }
}
