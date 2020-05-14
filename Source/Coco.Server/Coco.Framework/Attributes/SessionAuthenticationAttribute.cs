using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Coco.Framework.Attributes
{
    public class SessionAuthenticationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly bool _ignoreFilter;

        public SessionAuthenticationAttribute(bool ignoreFilter = false)
        {
            this._ignoreFilter = ignoreFilter;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            var actionDescriptor = filterContext.ActionDescriptor;
            var filterDescriptors = actionDescriptor.FilterDescriptors;

            //check whether this filter has been overridden for the action
            var actionFilter = filterDescriptors
                .FirstOrDefault(x => x.Scope == FilterScope.Action || x.Scope == FilterScope.Controller)
                .Filter;

            if (actionFilter != null && _ignoreFilter)
            {
                return;
            }

            var httpContext = filterContext.HttpContext;
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("/Authentication/Login");
            }
        }
    }
}
