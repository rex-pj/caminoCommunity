using Camino.Core.Contracts.Repositories.Orders;
using Camino.Core.Domains.Orders;
using Camino.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Camino.Core.Domains;
using Camino.Shared.Utils;
using Camino.Core.DependencyInjection;

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
            _orderNewStatus = OrderStatuses.New.GetCode();
            _dbContext = dbContext;
        }
        
        public async Task<IList<Order>> GetOrderByCustomerIdAsync(long customerId)
        {
            var newStatusId = _orderNewStatus;
            return await _orderRepository.Table
                .Where(x => x.CustomerId == customerId).ToListAsync();
        }

        public async Task<long> CreateOrderAsync(Order order)
        {
            order.CreatedDateUtc = DateTimeOffset.UtcNow;
            await _orderRepository.InsertAsync(order);
            await _dbContext.SaveChangesAsync();
            return order.Id;
        }

        public async Task<long> CreateOrderItemAsync(OrderItem orderItem)
        {
            await _orderItemRepository.InsertAsync(orderItem);
            await _dbContext.SaveChangesAsync();
            return orderItem.Id;
        }
    }
}
