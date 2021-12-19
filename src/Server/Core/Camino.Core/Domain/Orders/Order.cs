using System;
using System.Collections.Generic;

namespace Camino.Core.Domain.Orders
{
    public class Order
    {
        public long Id { get; set; }
        public string CustomOrderNumber { get; set; }
        public string BillingAddress { get; set; }
        public string CustomerId { get; set; }
        public string PickupAddress { get; set; }
        public string ShippingAddress { get; set; }
        public Guid OrderGuid { get; set; }
        public int StoreId { get; set; }
        public bool IsPickupInStore { get; set; }
        public int OrderStatusId { get; set; }
        public int ShippingStatusId { get; set; }
        public int PaymentStatusId { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal RefundedAmount { get; set; }
        public string CustomerIp { get; set; }
        public DateTimeOffset? PaidDateUtc { get; set; }
        public string ShippingMethod { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedOnUtc { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
