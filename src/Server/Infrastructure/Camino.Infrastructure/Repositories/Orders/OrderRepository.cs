using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Orders;
using Camino.Core.Domain.Orders;
using Camino.Core.Utils;
using Camino.Shared.Enums;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Requests.Orders;
using Camino.Shared.Results.Orders;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Infrastructure.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly int _orderNewStatus;

        public OrderRepository(IRepository<Order> orderRepository, IRepository<OrderItem> orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _orderNewStatus = OrderStatus.New.GetCode();
        }
        
        public async Task<IList<OrderResult>> GetOrderByCustomerIdAsync(long customerId, IdRequestFilter<long> filter)
        {
            var newStatusId = OrderStatus.New.GetCode();
            return await _orderRepository.Table
                .Where(x => x.CustomerId == customerId)
                .Where(x => (x.IsDeleted && filter.CanGetDeleted)
                            || (x.OrderStatusId != _orderNewStatus && filter.CanGetInactived)
                            || (x.OrderStatusId == _orderNewStatus))
                .Select(x => new OrderResult
                {
                    Id = x.Id,
                    IsPickupInStore = x.IsPickupInStore,
                    IsDeleted = x.IsDeleted,
                    CustomOrderNumber = x.CustomOrderNumber,
                    CustomerIp = x.CustomerIp,
                    CustomerId = x.CustomerId,
                    BillingAddress = x.BillingAddress,
                    CreatedOnUtc = x.CreatedDateUtc,
                    OrderDiscount = x.OrderDiscount,
                    OrderStatusId = x.OrderStatusId,
                    OrderTotal = x.OrderTotal,
                    PaidDateUtc = x.PaidDateUtc,
                    PaymentStatusId = x.PaymentStatusId,
                    PickupAddress = x.PickupAddress,
                    ShippingAddress = x.ShippingAddress,
                    ShippingMethod = x.ShippingMethod,
                    ShippingStatusId = x.ShippingStatusId,
                    StoreId = x.StoreId
                }).ToListAsync();
        }

        public async Task<IList<OrderItemResult>> GetOrderItemByOrderIdAsync(long orderId)
        {
            var newStatusId = OrderStatus.New.GetCode();
            return await _orderItemRepository.Table
                .Where(x => x.OrderId == orderId)
                .Select(x => new OrderItemResult
                {
                    Id = x.Id,
                    ItemWeight = x.ItemWeight,
                    OrderId = x.OrderId,
                    OrderItemGuid = x.OrderItemGuid,
                    OriginalProductCost = x.OriginalProductCost,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToListAsync();
        }

        public async Task<long> CreateOrderAsync(CreateOrderRequest request)
        {
            var orderId = await _orderRepository.AddWithInt64EntityAsync(new Order
            {
                BillingAddress = request.BillingAddress,
                CreatedDateUtc = request.CreatedDateUtc,
                CustomerId = request.CustomerId,
                CustomerIp = request.CustomerIp,
                CustomOrderNumber = request.CustomOrderNumber,
                IsDeleted = request.IsDeleted,
                IsPickupInStore = request.IsPickupInStore,
                OrderDiscount = request.OrderDiscount,
                OrderStatusId = request.OrderStatusId,
                OrderTotal = request.OrderTotal,
                PaidDateUtc = request.PaidDateUtc,
                PaymentStatusId = request.PaymentStatusId,
                PickupAddress = request.PickupAddress,
                ShippingAddress = request.ShippingAddress,
                ShippingMethod = request.ShippingMethod,
                StoreId = request.StoreId,
                ShippingStatusId = request.ShippingStatusId
            });

            return orderId;
        }

        public async Task<long> CreateOrderItemAsync(CreateOrderItemRequest request)
        {
            return await _orderItemRepository.AddWithInt64EntityAsync(new OrderItem
            {
                ItemWeight = request.ItemWeight,
                OrderId = request.OrderId,
                OrderItemGuid = request.OrderItemGuid,
                OriginalProductCost = request.OriginalProductCost,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });
        }
    }
}
