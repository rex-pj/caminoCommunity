using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Constants;
using Camino.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Infrastructure.Identity.Attributes
{
    public class TokenAnnonymousAttribute : TypeFilterAttribute
    {
        private readonly bool _ignoreFilter;
        public bool IgnoreFilter => _ignoreFilter;

        public TokenAnnonymousAttribute() : base(typeof(TokenAnnonymousFilter))
        {
            Arguments = Array.Empty<object>();
        }

        private class TokenAnnonymousFilter : IAsyncAuthorizationFilter
        {
            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var token = context.HttpContext.Request.Headers[HttpHeaders.HeaderAuthenticationAccessToken];
                if (string.IsNullOrEmpty(token))
                {
                    return;
                }

                var filterDescriptors = context.ActionDescriptor.FilterDescriptors;

                //check whether this filter has been overridden for the action
                var actionFilter = filterDescriptors
                    .Where(x => x.Scope == FilterScope.Action || x.Scope == FilterScope.Controller)
                    .Select(filterDescriptor => filterDescriptor.Filter).OfType<TokenAnnonymousFilter>()
                    .FirstOrDefault();

                if (actionFilter == null)
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

                    context.Result = new AcceptedResult();
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
