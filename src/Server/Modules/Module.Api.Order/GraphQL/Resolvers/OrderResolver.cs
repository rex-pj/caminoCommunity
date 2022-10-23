using Camino.Infrastructure.GraphQL.Resolvers;
using Module.Api.Order.GraphQL.Resolvers.Contracts;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Module.Api.Order.Models;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Application.Contracts.AppServices.Orders;
using Camino.Application.Contracts.AppServices.Orders.Dtos;
using Camino.Shared.Enums;
using Camino.Shared.Utils;

namespace Module.Api.Order.GraphQL.Resolvers
{
    public class OrderResolver : BaseResolver, IOrderResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IOrderAppService _orderAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderResolver(IUserManager<ApplicationUser> userManager, IOrderAppService orderAppService,
            IHttpContextAccessor httpContextAccessor)
            : base()
        {
            _userManager = userManager;
            _orderAppService = orderAppService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<long> AddToCartAsync(ClaimsPrincipal claimsPrincipal, AddToCartModel request)
        {
            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var currentIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            var orderId = await _orderAppService.AddToCartAsync(new AddToCartRequest
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
                PaymentStatusId = PaymentStatuses.Pending.GetCode(),
                Quantity = request.Quantity,
                ProductId = request.ProductId,
                ShippingStatusId = ShippingStatuses.NotYetShipped.GetCode()
            });

            return orderId;
        }
    }
}
