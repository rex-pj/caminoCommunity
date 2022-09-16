using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Constants;
using Camino.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Camino.Framework.Attributes
{
    public class TokenAuthenticationAttribute : TypeFilterAttribute
    {
        private readonly bool _ignoreFilter;
        public bool IgnoreFilter => _ignoreFilter;

        public TokenAuthenticationAttribute(bool ignoreFilter = false) : base(typeof(TokenAuthenticationAttribute))
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
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                var token = context.HttpContext.Request.Headers[HttpHeades.HeaderAuthenticationAccessToken];
                if (string.IsNullOrEmpty(token))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                var actionDescriptor = context.ActionDescriptor;
                var filterDescriptors = actionDescriptor.FilterDescriptors;

                //check whether this filter has been overridden for the action
                var actionFilter = filterDescriptors
                    .Where(x => x.Scope == FilterScope.Action || x.Scope == FilterScope.Controller)
                    .Select(filterDescriptor => filterDescriptor.Filter).OfType<TokenAuthenticationFilter>()
                    .FirstOrDefault();

                if (actionFilter != null && _ignoreFilter)
                {
                    return;
                }

                var httpContext = context.HttpContext;
                var requestServices = httpContext.RequestServices;
                var jwtHelper = requestServices.GetRequiredService<IJwtHelper>();
                var loginManager = requestServices.GetRequiredService<ILoginManager<ApplicationUser>>();
                var userManager = requestServices.GetRequiredService<IUserManager<ApplicationUser>>();

                try
                {
                    var claimsIdentity = await jwtHelper.ValidateTokenAsync(token);
                    if (!claimsIdentity.IsAuthenticated)
                    {
                        context.Result = new ForbidResult();
                    }

                    httpContext.User.AddIdentity(claimsIdentity);
                    var user = await userManager.GetUserAsync(httpContext.User);
                    await loginManager.SignInWithClaimsAsync(user, true, new Claim[] { new Claim("amr", "pwd") });
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
