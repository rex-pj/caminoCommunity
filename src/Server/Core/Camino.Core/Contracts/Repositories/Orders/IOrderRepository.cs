using Camino.Shared.Requests.Filters;
using Camino.Shared.Requests.Orders;
using Camino.Shared.Results.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<long> CreateOrderAsync(CreateOrderRequest request);
        Task<long> CreateOrderItemAsync(CreateOrderItemRequest request);
        Task<IList<OrderResult>> GetOrderByCustomerIdAsync(long customerId, IdRequestFilter<long> filter);
        Task<IList<OrderItemResult>> GetOrderItemByOrderIdAsync(long orderId);
    }
}
