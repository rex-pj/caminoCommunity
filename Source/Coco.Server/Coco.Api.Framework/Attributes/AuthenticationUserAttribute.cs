using Coco.Api.Framework.AccountIdentity.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Coco.Api.Framework.Attributes
{
    public class AuthenticationUserAttribute : TypeFilterAttribute
    {
        #region Properties
        public bool IgnoreFilter { get; }
        #endregion

        public AuthenticationUserAttribute(bool isIgnore = false) : base(typeof(AuthenticationFilter))
        {
            this.IgnoreFilter = isIgnore;
            this.Arguments = new object[] { isIgnore };
        }

        public class AuthenticationFilter : IAuthorizationFilter
        {
            #region Fields

            private readonly bool _ignoreFilter;
            private readonly IWorkContext _workContext;

            #endregion

            public AuthenticationFilter(IWorkContext workContext, bool ignoreFilter = true)
            {
                this._ignoreFilter = ignoreFilter;
                _workContext = workContext;
            }

            public void OnAuthorization(AuthorizationFilterContext filterContext)
            {
                if (filterContext == null)
                {
                    throw new ArgumentNullException(nameof(filterContext));
                }

                //check whether this filter has been overridden for the action
                var actionFilter = filterContext.ActionDescriptor.FilterDescriptors
                    .Where(filterDescriptor => filterDescriptor.Scope == FilterScope.Action)
                    .Select(filterDescriptor => filterDescriptor.Filter).OfType<AuthenticationUserAttribute>()
                    .FirstOrDefault();

                if (actionFilter != null && actionFilter.IgnoreFilter && _ignoreFilter)
                {
                    return;
                }

                //there is AuthorizeLoggedUserFilter, so check access
                if (filterContext.Filters.Any(filter => filter is AuthenticationFilter))
                {
                    var user = _workContext.CurrentUser;
                    if (user == null)
                    {
                        filterContext.Result = new ForbidResult();
                    }
                }
            }
        }
    }
}
