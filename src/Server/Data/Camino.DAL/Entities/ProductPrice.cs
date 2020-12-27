using System;

namespace Camino.DAL.Entities
{
    public class ProductPrice
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public int Price { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsDiscounted { get; set; }
        public DateTimeOffset PricedDate { get; set; }
        public virtual Product Product { get; set; }
    }
}
