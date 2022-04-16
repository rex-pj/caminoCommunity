using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.DependencyInjection;
using Camino.Core.Contracts.Repositories.Orders;
using Camino.Core.Domain.Orders;
using Camino.Core.Utils;
using Camino.Shared.Enums;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Requests.Orders;
using Camino.Shared.Results.Orders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Orders
{
    public class OrderRepository : IOrderRepository, IScopedDependency
    {
        private readonly IEntityRepository<Order> _orderRepository;
        private readonly IEntityRepository<OrderItem> _orderItemRepository;
        private readonly IDbContext _dbContext;
        private readonly int _orderNewStatus;

        public OrderRepository(IEntityRepository<Order> orderRepository, IEntityRepository<OrderItem> orderItemRepository,
            IDbContext dbContext)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _orderNewStatus = OrderStatus.New.GetCode();
            _dbContext = dbContext;
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
            var newOrder = new Order
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
            };

            await _orderRepository.InsertAsync(newOrder);
            await _dbContext.SaveChangesAsync();
            return newOrder.Id;
        }

        public async Task<long> CreateOrderItemAsync(CreateOrderItemRequest request)
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
            await _orderItemRepository.InsertAsync(orderItem);
            await _dbContext.SaveChangesAsync();
            return orderItem.Id;
        }
    }
}
