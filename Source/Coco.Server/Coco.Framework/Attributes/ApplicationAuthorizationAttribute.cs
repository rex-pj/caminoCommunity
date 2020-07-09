using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.User;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Framework.Attributes
{
    public class ApplicationAuthorizationAttribute : TypeFilterAttribute
    {
        //public bool IgnoreFilter { get; set; }
        //public string Policy { get; set; }
        //public string Roles { get; set; }

        public ApplicationAuthorizationAttribute(bool ignoreFilter = false, string policy = "", string roles = "") : base(typeof(ApplicationAuthorizationFilter))
        {
            //IgnoreFilter = ignoreFilter;
            Arguments = new object[] { ignoreFilter, policy, roles };
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
                    context.Result = new RedirectResult("/Authentication/Login");
                }

                var userManager = httpContext.RequestServices.GetRequiredService<IUserManager<ApplicationUser>>();
                var user = await userManager.GetUserAsync(httpContext.User);
                var userBusiness = httpContext.RequestServices.GetRequiredService<IUserBusiness>();
                var mapper = httpContext.RequestServices.GetRequiredService<IMapper>();

                var userDto = mapper.Map<UserDto>(user);
                var userPolicy = userBusiness.GetUserRolesAuthorizationPolicies(userDto);
                if (userPolicy == null)
                {
                    context.Result = new RedirectResult("/Authentication/Logout");
                    return;
                }

                var roles = new string[] { };
                if (!string.IsNullOrEmpty(Roles))
                {
                    roles = Roles.Trim().Split(",");
                }

                if (roles.Any())
                {
                    foreach (var role in roles)
                    {
                        if (userPolicy.Roles.Any(x => x.Name == role))
                        {
                            return;
                        }
                    }
                }

                var isUserHasPolicy = userPolicy.AuthorizationPolicies.Any(x => x.Name == Policy);
                if (isUserHasPolicy)
                {
                    return;
                }
                var isRoleHasPolicy = userPolicy.Roles.Any(x => x.AuthorizationPolicies.Any(a => a.Name == Policy));

                if (isRoleHasPolicy)
                {
                    return;
                }

                context.Result = new RedirectResult("/Authentication/Logout");
            }
        }
    }
}