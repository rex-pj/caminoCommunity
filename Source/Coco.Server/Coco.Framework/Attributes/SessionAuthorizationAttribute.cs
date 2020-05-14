using Coco.Entities.Domain.Auth;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coco.Framework.Attributes
{
    public class SessionAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public bool IgnoreFilter;
        public string Policy;
        public string Roles;
        public SessionAuthorizationAttribute(bool ignoreFilter = false, string policy = "", string roles = "")
        {
            IgnoreFilter = ignoreFilter;
            Policy = policy;
            Roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            var filterDescriptors = filterContext.ActionDescriptor.FilterDescriptors;

            //check whether this filter has been overridden for the action
            var actionFilter = filterDescriptors
                .FirstOrDefault(x => x.Scope == FilterScope.Action || x.Scope == FilterScope.Controller)
                .Filter;

            if (actionFilter != null && IgnoreFilter)
            {
                return;
            }

            var httpContext = filterContext.HttpContext;
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("/Authentication/Login");
            }

            var userPrincipalId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedUserId = long.Parse(userPrincipalId);
            var userManager = httpContext.RequestServices.GetRequiredService<IUserManager<ApplicationUser>>();
            var userPolicy = await userManager.GetRoleAuthorizationsAsync(loggedUserId);

            var isUserHasPolicy = userPolicy.AuthorizationPolicies.Any(x => x.Name == Policy);
            var isRoleHasPolicy = userPolicy.Roles.Any(x => x.AuthorizationPolicies.Any(a => a.Name == Policy));
            if (!isUserHasPolicy && !isRoleHasPolicy)
            {
                filterContext.Result = new RedirectResult("/Authentication/Login");
            }
        }
    }
}
