using Coco.Business.Contracts;
using Coco.Api.Framework.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Coco.Api.Framework.Attributes
{
    public class AuthorizeAdminAttribute : TypeFilterAttribute
    {
        #region Properties
        public bool IgnoreFilter { get; }
        #endregion

        public AuthorizeAdminAttribute(bool isIgnore = false) : base(typeof(AuthorizeAdminFilter))
        {
            this.IgnoreFilter = isIgnore;
            this.Arguments = new object[] { isIgnore };
        }

        public class AuthorizeAdminFilter : IAuthorizationFilter
        {
            #region Fields

            private readonly bool _ignoreFilter;
            private readonly IRoleBusiness _roleBusiness;

            #endregion

            public AuthorizeAdminFilter(IRoleBusiness roleBusiness, bool ignoreFilter = true)
            {
                this._ignoreFilter = ignoreFilter;
                this._roleBusiness = roleBusiness;
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
                    var user = new CustomClaimsPrincipal(filterContext.HttpContext.User, _roleBusiness);
                    if (user != null)
                    {
                        var isAdmin = user.IsInRole("admin");
                        if (!isAdmin)
                        {
                            filterContext.Result = new ForbidResult();
                        }
                    }
                }
            }
        }
    }
}
