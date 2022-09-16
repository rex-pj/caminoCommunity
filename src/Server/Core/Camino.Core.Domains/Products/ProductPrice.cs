using System;

namespace Camino.Core.Domains.Products
{
    public class ProductPrice
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsDiscounted { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PricedDate { get; set; }
        public virtual Product Product { get; set; }
    }
}
