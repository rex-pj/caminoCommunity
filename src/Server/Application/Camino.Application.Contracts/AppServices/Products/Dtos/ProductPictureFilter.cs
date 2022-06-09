namespace Camino.Application.Contracts.AppServices.Products.Dtos
{
    public class ProductPictureFilter : BaseFilter
    {
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public string MimeType { get; set; }
    }
}
