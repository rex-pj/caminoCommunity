namespace Camino.Core.Domains.Products
{
    public class ProductCategoryRelation
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
