using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Orders;
using Camino.Core.Domain.Orders;
using Camino.Shared.Requests.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Camino.Infrastructure.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;

        public OrderRepository(IRepository<Order> orderRepository, IRepository<OrderItem> orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<long> CreateOrder(CreateOrderRequest request)
        {
            var orderId = await _orderRepository.AddWithInt64EntityAsync(new Order
            {
                BillingAddress = request.BillingAddress,
                CreatedOnUtc = request.CreatedOnUtc,
                CustomerId = request.CustomerId,
                CustomerIp = request.CustomerIp,
                CustomOrderNumber = request.CustomOrderNumber,
                IsDeleted = request.IsDeleted,
                IsPickupInStore = request.IsPickupInStore,
                OrderDiscount = request.OrderDiscount,
                OrderGuid = request.OrderGuid,
                OrderStatusId = request.OrderStatusId,
                OrderTotal = request.OrderTotal,
                PaidDateUtc = request.PaidDateUtc,
                PaymentStatusId = request.PaymentStatusId,
                PickupAddress = request.PickupAddress,
                ShippingAddress = request.ShippingAddress,
                RefundedAmount = request.RefundedAmount,
                ShippingMethod = request.ShippingMethod,
                StoreId = request.StoreId,
                ShippingStatusId = request.ShippingStatusId
            });

            if (orderId > 0)
            {
                await _orderItemRepository.AddWithInt64EntityAsync(request.OrderItems.Select(x => new OrderItem
                {
                    ItemWeight = x.ItemWeight,
                    OrderId = orderId,
                    OrderItemGuid = Guid.NewGuid(),
                    OriginalProductCost = x.OriginalProductCost,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList());

                return orderId;
            }

            return -1;
        }
    }
}
