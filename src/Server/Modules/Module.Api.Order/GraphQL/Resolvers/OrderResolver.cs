using Camino.Framework.GraphQL.Resolvers;
using Camino.Core.Domain.Identities;
using Camino.Core.Contracts.IdentityManager;
using Module.Api.Order.GraphQL.Resolvers.Contracts;
using Camino.Core.Contracts.Services.Orders;

namespace Module.Api.Order.GraphQL.Resolvers
{
    public class OrderResolver : BaseResolver, IOrderResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;

        public OrderResolver(IUserManager<ApplicationUser> userManager, IOrderService orderService)
            : base()
        {
            _userManager = userManager;
            _orderService = orderService;
        }

    }
}
