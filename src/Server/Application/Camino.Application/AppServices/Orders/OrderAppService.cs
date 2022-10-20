using Camino.Core.Contracts.Repositories.Orders;
using Camino.Shared.Enums;
using Camino.Application.Contracts.AppServices.Orders;
using Camino.Core.DependencyInjection;
using Camino.Application.Contracts.AppServices.Orders.Dtos;
using Camino.Core.Domains.Orders;
using Camino.Shared.Utils;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Camino.Application.AppServices.Articles
{
    public class OrderAppService : IOrderAppService, IScopedDependency
    {
        private readonly IOrderRepository _orderRepository;

        public OrderAppService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<long> AddToCartAsync(AddToCartRequest request)
        {
            var currentOrders = await _orderRepository.GetOrderByCustomerIdAsync(request.CustomerId);
            var orderItem = new CreateOrderItemRequest
            {
                CustomerId = request.CustomerId,
                ItemWeight = request.ItemWeight,
                OrderItemGuid = request.OrderItemGuid,
                OriginalProductCost = request.OriginalProductCost,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            if (currentOrders == null || !currentOrders.Any())
            {
                // create card
                var modifiedDateUtc = DateTime.UtcNow;
                var orderId = await _orderRepository.CreateOrderAsync(new Order
                {
                    CreatedDateUtc = modifiedDateUtc,
                    IsDeleted = false,
                    OrderStatusId = request.OrderStatusId,
                    BillingAddress = request.BillingAddress,
                    CustomerId = request.CustomerId,
                    CustomerIp = request.CustomerIp,
                    CustomOrderNumber = request.CustomOrderNumber,
                    IsPickupInStore = request.IsPickupInStore,
                    OrderDiscount = request.OrderDiscount,
                    OrderTotal = request.OrderTotal,
                    PaymentStatusId = PaymentStatuses.Pending.GetCode(),
                    PickupAddress = request.PickupAddress,
                    ShippingAddress = request.ShippingAddress,
                    ShippingMethod = request.ShippingMethod,
                    StoreId = request.StoreId,
                    ShippingStatusId = ShippingStatuses.NotYetShipped.GetCode()
                });

                // then create Card Item
                orderItem.OrderId = orderId;
                await CreateOrderItemAsync(orderItem);
                return orderId;
            }

            var currentOrder = currentOrders.FirstOrDefault();
            orderItem.OrderId = currentOrder.Id;
            await CreateOrderItemAsync(orderItem);

            return currentOrder.Id;
        }

        private async Task CreateOrderItemAsync(CreateOrderItemRequest request)
        {
            var orderItem = new OrderItem
            {
                ItemWeight = request.ItemWeight,
                OrderId = request.OrderId,
                OrderItemGuid = request.OrderItemGuid,
                OriginalProductCost = request.OriginalProductCost,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            await _orderRepository.CreateOrderItemAsync(orderItem);
        }
    }
}
