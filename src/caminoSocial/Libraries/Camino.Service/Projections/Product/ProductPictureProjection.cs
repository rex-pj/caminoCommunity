using System;

namespace Camino.Service.Projections.Product
{
    public class ProductPictureProjection
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductPictureType { get; set; }
        public long PictureId { get; set; }
        public string PictureName { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset PictureCreatedDate { get; set; }
        public long PictureCreatedById { get; set; }
        public string PictureCreatedBy { get; set; }
    }
}
