using Camino.Core.Domain.Media;

namespace Camino.Core.Domain.Articles
{
    public class ArticlePicture
    {
        public long Id { get; set; }
        public long ArticleId { get; set; }
        public long PictureId { get; set; }
        public int PictureTypeId { get; set; }
        public virtual Article Article { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
