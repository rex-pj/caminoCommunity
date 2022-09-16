namespace Camino.Application.Contracts.AppServices.Products.Dtos
{
    public class ProductPictureFilter : BaseFilter
    {
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public string MimeType { get; set; }
    }
}
