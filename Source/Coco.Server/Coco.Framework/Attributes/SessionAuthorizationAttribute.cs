using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Coco.Framework.Attributes
{
    public class SessionAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly bool _ignoreFilter;
        private readonly string _policy;

        public SessionAuthorizationAttribute(bool ignoreFilter = false, string policy = "")
        {
            _ignoreFilter = ignoreFilter;
            _policy = policy;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
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
