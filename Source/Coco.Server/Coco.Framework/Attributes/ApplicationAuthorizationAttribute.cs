using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.User;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Framework.Attributes
{
    public class ApplicationAuthorizationAttribute : TypeFilterAttribute
    {
        public bool IgnoreFilter { get; set; }
        public string Policy { get; set; }
        public string Roles { get; set; }


        public ApplicationAuthorizationAttribute(bool ignoreFilter = false, string policy = "", string roles = "") : base(typeof(ApplicationAuthorizationFilter))
        {
            IgnoreFilter = ignoreFilter;
            Policy = policy;
            Roles = roles;
        }

        private class ApplicationAuthorizationFilter : IAsyncAuthorizationFilter
        {
            private readonly bool _ignoreFilter;
            public string Roles { get; set; }
            public string Policy { get; set; }
            private string[] _roles;


            public ApplicationAuthorizationFilter(bool ignoreFilter = false, string policy = "", string roles = "")
            {
                Policy = policy;
                Roles = roles;
                _ignoreFilter = ignoreFilter;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                var filterDescriptors = context.ActionDescriptor.FilterDescriptors;
                //check whether this filter has been overridden for the action
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
                var userPolicy = userBusiness.GetRoleAuthorizationPolicies(userDto);
                if (userPolicy == null)
                {
                    context.Result = new RedirectResult("/Authentication/Logout");
                    return;
                }

                if (!string.IsNullOrEmpty(Roles))
                {
                    _roles = Roles.Trim().Split(",");
                }

                if (_roles.Any())
                {
                    foreach (var role in _roles)
                    {
                        if (userPolicy.Roles.Any(x => x.Name == role))
                        {
                            return;
                        }
                    }
                }

                var isUserHasPolicy = userPolicy.AuthorizationPolicies.Any(x => x.Name == Policy);
                var isRoleHasPolicy = userPolicy.Roles.Any(x => x.AuthorizationPolicies.Any(a => a.Name == Policy));
                if (!isUserHasPolicy && !isRoleHasPolicy)
                {
                    context.Result = new RedirectResult("/Authentication/Logout");
                }
            }
        }
    }
}