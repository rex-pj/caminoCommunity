using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Coco.Api.Framework.Attributes
{
    public class AuthorizeLoggedUserAttribute : TypeFilterAttribute
    {
        #region Properties
        public bool IgnoreFilter { get; }
        #endregion

        public AuthorizeLoggedUserAttribute(bool isIgnore = false) : base(typeof(AuthorizeAdminFilter))
        {
            this.IgnoreFilter = isIgnore;
            this.Arguments = new object[] { isIgnore };
        }

        public class AuthorizeAdminFilter : IAuthorizationFilter
        {
            #region Fields

            private readonly bool _ignoreFilter;

            #endregion

            public AuthorizeAdminFilter(bool ignoreFilter = true)
            {
                this._ignoreFilter = ignoreFilter;
            }

            public void OnAuthorization(AuthorizationFilterContext filterContext)
            {
                if (filterContext == null)
                {
                    throw new ArgumentNullException(nameof(filterContext));
                }

                //check whether this filter has been overridden for the action
                AuthorizeAdminAttribute actionFilter = filterContext.ActionDescriptor.FilterDescriptors
                    .Where(filterDescriptor => filterDescriptor.Scope == FilterScope.Action)
                    .Select(filterDescriptor => filterDescriptor.Filter).OfType<AuthorizeAdminAttribute>()
                    .FirstOrDefault();

                if (actionFilter != null && actionFilter.IgnoreFilter && _ignoreFilter)
                {
                    return;
                }

                //there is AdminAuthorizeFilter, so check access
                if (filterContext.Filters.Any(filter => filter is AuthorizeAdminFilter))
                {
                    var user = filterContext.HttpContext;
                    if (user != null)
                    {
                        filterContext.Result = new ForbidResult();
                    }
                }
            }
        }
    }
}
