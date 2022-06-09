using Camino.Application.Contracts.AppServices.Orders.Dtos;

namespace Camino.Application.Contracts.AppServices.Orders
{
    public interface IOrderAppService
    {
        Task<long> AddToCartAsync(AddToCartRequest request);
    }
}
