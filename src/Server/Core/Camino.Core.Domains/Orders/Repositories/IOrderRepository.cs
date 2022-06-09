using Camino.Core.Domains.Orders;

namespace Camino.Core.Contracts.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<long> CreateOrderAsync(Order order);
        Task<long> CreateOrderItemAsync(OrderItem orderItem);
        Task<IList<Order>> GetOrderByCustomerIdAsync(long customerId);
    }
}
