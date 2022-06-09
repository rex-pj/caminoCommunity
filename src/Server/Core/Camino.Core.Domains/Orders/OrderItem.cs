using System;

namespace Camino.Core.Domains.Orders
{
    public class OrderItem
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public Guid OrderItemGuid { get; set; }
        public int Quantity { get; set; }
        public decimal OriginalProductCost { get; set; }
        public decimal ItemWeight { get; set; }
        public virtual Order Order { get; set; }
    }
}
