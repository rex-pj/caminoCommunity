using Camino.Framework.GraphQL.Resolvers;
using Camino.Core.Domain.Identities;
using Camino.Core.Contracts.IdentityManager;
using Module.Api.Order.GraphQL.Resolvers.Contracts;
using Camino.Core.Contracts.Services.Orders;
using System.Threading.Tasks;
using Camino.Shared.Requests.Orders;
using System;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Module.Api.Order.Models;

namespace Module.Api.Order.GraphQL.Resolvers
{
    public class OrderResolver : BaseResolver, IOrderResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderResolver(IUserManager<ApplicationUser> userManager, IOrderService orderService,
            IHttpContextAccessor httpContextAccessor)
            : base()
        {
            _userManager = userManager;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<long> AddToCartAsync(ClaimsPrincipal claimsPrincipal, AddToCartModel request)
        {
            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var currentIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            var orderId = await _orderService.AddToCartAsync(new AddToCartRequest
            {
                OrderGuid = Guid.NewGuid(),
                OrderStatusId = request.OrderStatusId,
                BillingAddress = request.BillingAddress,
                CustomerId = currentUserId,
                CustomerIp = currentIp.ToString(),
                CustomOrderNumber = request.CustomOrderNumber,
                IsPickupInStore = request.IsPickupInStore,
                OrderDiscount = request.OrderDiscount,
                OrderTotal = request.OrderTotal,
                PickupAddress = request.PickupAddress,
                ShippingAddress = request.ShippingAddress,
                ShippingMethod = request.ShippingMethod,
                StoreId = request.StoreId,
                OrderItemGuid = Guid.NewGuid(),
                ItemWeight = request.ItemWeight,
                OriginalProductCost = request.OriginalProductCost,
                PaymentStatusId = PaymentStatus.Pending.GetCode(),
                Quantity = request.Quantity,
                ProductId = request.ProductId,
                ShippingStatusId = ShippingStatus.NotYetShipped.GetCode()
            });

            return orderId;
        }
    }
}
