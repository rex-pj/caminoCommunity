namespace Camino.Application.Contracts.AppServices.Products.Dtos
{
    public class ProductPictureResult
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductPictureTypeId { get; set; }
        public long PictureId { get; set; }
        public string PictureName { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset PictureCreatedDate { get; set; }
        public long PictureCreatedById { get; set; }
        public string PictureCreatedBy { get; set; }
    }
}
