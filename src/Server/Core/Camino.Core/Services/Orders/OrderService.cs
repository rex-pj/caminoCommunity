using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Contracts.Services.Orders;
using Camino.Core.Contracts.Repositories.Orders;
using Camino.Core.Contracts.Repositories.Products;

namespace Camino.Services.Articles
{
    public class OrderService : IOrderService
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

    }
}
