using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Coco.Framework.Attributes
{
    public class ApplicationAuthenticationAttribute : TypeFilterAttribute
    {
        private readonly bool _ignoreFilter;
        public bool IgnoreFilter => _ignoreFilter;

        public ApplicationAuthenticationAttribute(bool ignoreFilter = false) : base(typeof(ApplicationAuthenticationFilter))
        {
            _ignoreFilter = ignoreFilter;
            Arguments = new object[] { ignoreFilter };
        }

        private class ApplicationAuthenticationFilter : IAuthorizationFilter
        {
            private readonly bool _ignoreFilter;

            public ApplicationAuthenticationFilter(bool ignoreFilter)
            {
                _ignoreFilter = ignoreFilter;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                var actionDescriptor = context.ActionDescriptor;
                var filterDescriptors = actionDescriptor.FilterDescriptors;

                //check whether this filter has been overridden for the action
                var actionFilter = filterDescriptors
                    .Where(x => x.Scope == FilterScope.Action || x.Scope == FilterScope.Controller)
                    .Select(filterDescriptor => filterDescriptor.Filter).OfType<ApplicationAuthenticationFilter>()
                    .FirstOrDefault();

                if (actionFilter != null && _ignoreFilter)
                {
                    return;
                }

                var httpContext = context.HttpContext;
                if (!httpContext.User.Identity.IsAuthenticated)
                {
                    context.Result = new RedirectResult("/Authentication/Login");
                }
            }
        }
    }
}
