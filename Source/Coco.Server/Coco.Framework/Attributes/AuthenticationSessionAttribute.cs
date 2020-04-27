using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Coco.Framework.Attributes
{
    public class AuthenticationSessionAttribute : TypeFilterAttribute
    {
        #region Properties
        public bool IgnoreFilter { get; }
        #endregion

        public AuthenticationSessionAttribute(bool isIgnore = false) : base(typeof(AuthenticationFilter))
        {
            this.IgnoreFilter = isIgnore;
            this.Arguments = new object[] { isIgnore };
        }

        public class AuthenticationFilter : IAuthorizationFilter
        {
            #region Fields
            private readonly bool _ignoreFilter;
            #endregion

            public AuthenticationFilter(bool ignoreFilter = true)
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

                var authenticationFilter = actionFilter as AuthenticationSessionAttribute;
                if (authenticationFilter != null && authenticationFilter.IgnoreFilter && _ignoreFilter)
                {
                    return;
                }

                if(!filterContext.Filters.Any(filter => filter is AuthenticationFilter))
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
}
