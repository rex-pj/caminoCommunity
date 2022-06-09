using System;

namespace Camino.Application.Contracts.AppServices.Orders.Dtos
{
    public class OrderItemResult
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public Guid OrderItemGuid { get; set; }
        public int Quantity { get; set; }
        public decimal OriginalProductCost { get; set; }
        public decimal ItemWeight { get; set; }
    }
}
