namespace Camino.Shared.Requests.Products
{
    public class ProductCategoryRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UpdatedById { get; set; }
        public long CreatedById { get; set; }
        public int? ParentId { get; set; }
    }
}
