using Camino.Application.Contracts.AppServices.Media.Dtos;

namespace Camino.Application.Contracts.AppServices.Articles.Dtos
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
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public PictureRequest Picture { get; set; }
    }
}
