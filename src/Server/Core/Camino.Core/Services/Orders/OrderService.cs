using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Contracts.Services.Orders;
using Camino.Core.Contracts.Repositories.Orders;
using Camino.Core.Contracts.Repositories.Products;
using System.Threading.Tasks;
using Camino.Shared.Requests.Orders;
using Camino.Shared.Requests.Filters;
using System.Linq;
using System;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Core.Contracts.DependencyInjection;

namespace Camino.Services.Articles
{
    public class OrderService : IOrderService, IScopedDependency
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository orderRepository,
            IUserRepository userRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<long> AddToCartAsync(AddToCartRequest request)
        {
            var currentOrders = await _orderRepository.GetOrderByCustomerIdAsync(request.CustomerId, new IdRequestFilter<long>());
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
                var modifiedDateUtc = DateTimeOffset.UtcNow;
                var orderId = await _orderRepository.CreateOrderAsync(new CreateOrderRequest
                {
                    CreatedDateUtc = modifiedDateUtc,
                    IsDeleted = false,
                    OrderGuid = request.OrderGuid,
                    OrderStatusId = request.OrderStatusId,
                    BillingAddress = request.BillingAddress,
                    CustomerId = request.CustomerId,
                    CustomerIp = request.CustomerIp,
                    CustomOrderNumber = request.CustomOrderNumber,
                    IsPickupInStore = request.IsPickupInStore,
                    OrderDiscount = request.OrderDiscount,
                    OrderTotal = request.OrderTotal,
                    PaymentStatusId = PaymentStatus.Pending.GetCode(),
                    PickupAddress = request.PickupAddress,
                    ShippingAddress = request.ShippingAddress,
                    ShippingMethod = request.ShippingMethod,
                    StoreId = request.StoreId,
                    ShippingStatusId = ShippingStatus.NotYetShipped.GetCode()
                });

                // then create Card Item
                orderItem.OrderId = orderId;
                await _orderRepository.CreateOrderItemAsync(orderItem);
                return orderId;
            }

            var currentOrder = currentOrders.FirstOrDefault();
            orderItem.OrderId = currentOrder.Id;
            await _orderRepository.CreateOrderItemAsync(orderItem);

            return currentOrder.Id;
        }
    }
}
