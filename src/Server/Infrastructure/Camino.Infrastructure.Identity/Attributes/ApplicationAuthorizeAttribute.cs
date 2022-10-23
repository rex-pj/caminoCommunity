using Camino.Application.Contracts.AppServices.Authentication;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Infrastructure.Identity.Attributes
{
    public class ApplicationAuthorizeAttribute : TypeFilterAttribute
    {
        public ApplicationAuthorizeAttribute(bool ignoreFilter = false, string policy = "", string roles = "")
            : base(typeof(ApplicationAuthorizationFilter))
        {
            Arguments = new object[] { ignoreFilter, policy, roles };
        }

        public ApplicationAuthorizeAttribute(string policy = "", string roles = "")
            : this(false, policy, roles)
        {

        }

        private class ApplicationAuthorizationFilter : IAsyncAuthorizationFilter
        {
            private readonly bool _ignoreFilter;
            public string Roles { get; set; }
            public string Policy { get; set; }

            public ApplicationAuthorizationFilter(bool ignoreFilter = false, string policy = "", string roles = "")
            {
                Policy = policy;
                Roles = roles;
                _ignoreFilter = ignoreFilter;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var filterDescriptors = context.ActionDescriptor.FilterDescriptors;
                var actionFilter = filterDescriptors
                    .FirstOrDefault(x => x.Scope == FilterScope.Action || x.Scope == FilterScope.Controller)
                    .Filter;

                if (actionFilter != null && _ignoreFilter)
                {
                    return;
                }

                var httpContext = context.HttpContext;
                if (!httpContext.User.Identity.IsAuthenticated)
                {
                    context.Result = new RedirectResult("/Authentication/NoAuthorization");
                }

                var requestServices = httpContext.RequestServices;
                var userManager = requestServices.GetRequiredService<IUserManager<ApplicationUser>>();
                var authenticationService = requestServices.GetRequiredService<IAuthenticationAppService>();
                var user = await userManager.GetUserAsync(httpContext.User);

                // Authorize by authorization policies in current user or in current user's roles
                var hasPolicy = await userManager.HasPolicyAsync(user, Policy);
                if (hasPolicy)
                {
                    return;
                }

                var userRoles = authenticationService.GetUserRoles(user.Id);
                if (userRoles == null || !userRoles.Any())
                {
                    context.Result = new RedirectResult("/Authentication/NoAuthorization");
                    return;
                }

                var roles = new string[] { };
                if (!string.IsNullOrEmpty(Roles))
                {
                    roles = Roles.Trim().Split(",");
                }

                // Authorize by roles
                var hasRoles = roles.Join(userRoles, r => r, ur => ur.RoleName, (r, ur) => ur).Any();
                if (!hasRoles)
                {
                    context.Result = new RedirectResult("/Authentication/NoAuthorization");
                    return;
                }
            }
        }
    }
}