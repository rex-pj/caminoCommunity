using System;

namespace Module.Order.Api.Models
{
    public class AddToCartModel
    {
        public string CustomOrderNumber { get; set; }
        public string BillingAddress { get; set; }
        public long CustomerId { get; set; }
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
        public string ShippingMethod { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public Guid OrderItemGuid { get; set; }
        public int Quantity { get; set; }
        public decimal OriginalProductCost { get; set; }
        public decimal ItemWeight { get; set; }
        public string CustomerIp { get; set; }
    }
}
