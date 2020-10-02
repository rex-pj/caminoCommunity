namespace Camino.DAL.Entities
{
    public class ProductPicture
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long PictureId { get; set; }
        public int PictureType { get; set; }
        public virtual Product Product { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
