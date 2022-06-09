namespace Camino.Application.Contracts.AppServices.Products.Dtos
{
    public class ProductAttributeModifyRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long CreatedById { get; set; }
        public long UpdatedById { get; set; }
    }
}
