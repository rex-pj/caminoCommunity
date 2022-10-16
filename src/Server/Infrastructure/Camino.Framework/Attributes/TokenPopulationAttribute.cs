using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Constants;
using Camino.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Camino.Framework.Attributes
{
    public class TokenPopulationAttribute : TypeFilterAttribute
    {
        private readonly bool _ignoreFilter;
        public bool IgnoreFilter => _ignoreFilter;

        public TokenPopulationAttribute(bool ignoreFilter = false) : base(typeof(TokenPopulationFilter))
        {
            _ignoreFilter = ignoreFilter;
            Arguments = new object[] { ignoreFilter };
        }

        private class TokenPopulationFilter : IAsyncAuthorizationFilter
        {
            private readonly bool _ignoreFilter;

            public TokenPopulationFilter(bool ignoreFilter)
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
                    .Select(filterDescriptor => filterDescriptor.Filter).OfType<TokenPopulationFilter>()
                    .FirstOrDefault();

                if (actionFilter != null && _ignoreFilter)
                {
                    return;
                }

                try
                {
                    var httpContext = context.HttpContext;
                    var requestServices = httpContext.RequestServices;
                    var jwtHelper = requestServices.GetRequiredService<IJwtHelper>();
                    var loginManager = requestServices.GetRequiredService<ILoginManager<ApplicationUser>>();
                    var userManager = requestServices.GetRequiredService<IUserManager<ApplicationUser>>();

                    var claimsIdentity = await jwtHelper.ValidateTokenAsync(token);
                    if (!claimsIdentity.IsAuthenticated)
                    {
                        return;
                    }

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
