using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Constants;
using Camino.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Camino.Infrastructure.Identity.Attributes
{
    public class TokenIdentityPopulationAttribute : TypeFilterAttribute
    {
        private readonly bool _ignoreFilter;
        public bool IgnoreFilter => _ignoreFilter;

        public TokenIdentityPopulationAttribute(bool ignoreFilter = false) : base(typeof(TokenIdentityPopulationFilter))
        {
            _ignoreFilter = ignoreFilter;
            Arguments = new object[] { ignoreFilter };
        }

        private class TokenIdentityPopulationFilter : IAsyncAuthorizationFilter
        {
            private readonly bool _ignoreFilter;

            public TokenIdentityPopulationFilter(bool ignoreFilter)
            {
                _ignoreFilter = ignoreFilter;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var token = context?.HttpContext.Request.Headers[HttpHeades.HeaderAuthenticationAccessToken];
                if (string.IsNullOrEmpty(token))
                {
                    return;
                }

                var filterDescriptors = context.ActionDescriptor.FilterDescriptors;

                //check whether this filter has been overridden for the action
                var actionFilter = filterDescriptors
                    .Where(x => x.Scope == FilterScope.Action || x.Scope == FilterScope.Controller)
                    .Select(filterDescriptor => filterDescriptor.Filter).OfType<TokenIdentityPopulationFilter>()
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
                        return;
                    }

                    var loginManager = requestServices.GetRequiredService<ILoginManager<ApplicationUser>>();
                    httpContext.User.AddIdentity(claimsIdentity);
                    var userIdentityId = httpContext.User.FindFirstValue(HttpHeades.UserIdentityClaimKey);
                    var user = await userManager.FindByIdentityIdAsync(userIdentityId);
                    await loginManager.SignInWithClaimsAsync(user, true, new Claim[] { new Claim("amr", "pwd") });
                    return;
                }
                catch (CaminoAuthenticationException)
                {
                    return;
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
