using Camino.Shared.Requests.Orders;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Orders
{
    public interface IOrderService
    {
        Task<long> AddToCartAsync(AddToCartRequest request);
    }
}
