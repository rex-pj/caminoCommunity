using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Constants;
using Camino.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Camino.Infrastructure.Identity.Attributes
{
    public class TokenAuthenticationAttribute : TypeFilterAttribute
    {
        private readonly bool _ignoreFilter;
        public bool IgnoreFilter => _ignoreFilter;

        public TokenAuthenticationAttribute(bool ignoreFilter = false) : base(typeof(TokenAuthenticationFilter))
        {
            _ignoreFilter = ignoreFilter;
            Arguments = new object[] { ignoreFilter };
        }

        private class TokenAuthenticationFilter : IAsyncAuthorizationFilter
        {
            private readonly bool _ignoreFilter;

            public TokenAuthenticationFilter(bool ignoreFilter)
            {
                _ignoreFilter = ignoreFilter;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var token = context.HttpContext.Request.Headers[HttpHeaders.HeaderAuthenticationAccessToken];
                if (string.IsNullOrEmpty(token))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                var filterDescriptors = context.ActionDescriptor.FilterDescriptors;

                //check whether this filter has been overridden for the action
                var actionFilter = filterDescriptors
                    .Where(x => x.Scope == FilterScope.Action || x.Scope == FilterScope.Controller)
                    .Select(filterDescriptor => filterDescriptor.Filter).OfType<TokenAuthenticationFilter>()
                    .FirstOrDefault();

                if (actionFilter != null && _ignoreFilter)
                {
                    return;
                }

                try
                {
                    var httpContext = context.HttpContext;
                    var requestServices = httpContext.RequestServices;
                    var userManager = requestServices.GetRequiredService<IUserManager<ApplicationUser>>();

                    var claimsIdentity = await userManager.ValidateTokenAsync(token);
                    if (!claimsIdentity.IsAuthenticated)
                    {
                        context.Result = new ForbidResult();
                    }

                    var userIdentityId = httpContext.User.FindFirstValue(HttpHeaders.UserIdentityClaimKey);
                    if (string.IsNullOrEmpty(userIdentityId))
                    {
                        context.Result = new ForbidResult();
                    }

                    var userId = await userManager.DecryptUserIdAsync(userIdentityId);
                    if (userId == 0)
                    {
                        context.Result = new ForbidResult();
                    }

                    return;
                }
                catch (CaminoAuthenticationException)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                    return;
                }
                catch (Exception)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    return;
                }
            }
        }
    }
}
