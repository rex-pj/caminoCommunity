namespace Camino.Core.Domains.Products
{
    public class ProductAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual ICollection<ProductAttributeRelation> ProductAttributeRelations { get; set; }
    }
}
