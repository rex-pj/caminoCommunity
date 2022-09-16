using System;

namespace Camino.Application.Contracts.AppServices.Orders.Dtos
{
    public class OrderResult
    {
        public long Id { get; set; }
        public string CustomOrderNumber { get; set; }
        public string BillingAddress { get; set; }
        public long CustomerId { get; set; }
        public string PickupAddress { get; set; }
        public string ShippingAddress { get; set; }
        public int StoreId { get; set; }
        public bool IsPickupInStore { get; set; }
        public int OrderStatusId { get; set; }
        public int ShippingStatusId { get; set; }
        public int PaymentStatusId { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal OrderTotal { get; set; }
        public string CustomerIp { get; set; }
        public DateTime? PaidDateUtc { get; set; }
        public string ShippingMethod { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
