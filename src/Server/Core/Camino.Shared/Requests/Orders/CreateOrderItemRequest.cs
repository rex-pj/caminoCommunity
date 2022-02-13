using System;

namespace Camino.Shared.Requests.Orders
{
    public class CreateOrderItemRequest
    {
        public long CustomerId { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public Guid OrderItemGuid { get; set; }
        public int Quantity { get; set; }
        public decimal OriginalProductCost { get; set; }
        public decimal ItemWeight { get; set; }
    }
}
