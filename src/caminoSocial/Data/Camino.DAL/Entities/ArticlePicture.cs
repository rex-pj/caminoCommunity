namespace Camino.DAL.Entities
{
    public class ArticlePicture
    {
        public long Id { get; set; }
        public long ArticleId { get; set; }
        public long PictureId { get; set; }
        public int PictureType { get; set; }
        public virtual Article Article { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
