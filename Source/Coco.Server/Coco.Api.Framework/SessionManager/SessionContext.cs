using Coco.Api.Framework.SessionManager.Contracts;
using Coco.Api.Framework.Commons.Constants;
using Coco.Api.Framework.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Coco.Api.Framework.SessionManager
{
    public class SessionContext : ISessionContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManager<ApplicationUser> _userManager;
        public readonly HttpContext HttpContext;
        public readonly HttpRequest HttpRequest;
        public readonly IHeaderDictionary RequestHeaders;
        public string AuthenticationToken { get; protected set; }

        public SessionContext(IHttpContextAccessor httpContextAccessor, IUserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            HttpContext = _httpContextAccessor.HttpContext;

            if (HttpContext != null)
            {
                HttpRequest = HttpContext.Request;
            }

            if (HttpRequest != null)
            {
                RequestHeaders = HttpRequest.Headers;
            }
        }

        private ApplicationUser _currentUser;

        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        public ApplicationUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    return GetLoggedUser();
                }

                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        }

        protected ApplicationUser GetLoggedUser()
        {
            var headerParams = GetAuthorizationHeaders();

            ApplicationUser user = null;
            if (headerParams != null
                && !string.IsNullOrEmpty(headerParams.AuthenticationToken)
                && !string.IsNullOrEmpty(headerParams.UserIdentityId))
            {
                user = _userManager
                    .GetLoggingUser(headerParams.UserIdentityId, headerParams.AuthenticationToken);

                AuthenticationToken = headerParams.AuthenticationToken;
                user.UserIdentityId = headerParams.UserIdentityId;
            }

            return user;
        }

        public SessionContextHeaders GetAuthorizationHeaders()
        {
            var httpHeaders = RequestHeaders;

            if (httpHeaders == null)
            {
                return null;
            }

            //var authenticationToken = httpHeaders.HeaderAuthorization;

            var userIdentityIds = httpHeaders.GetCommaSeparatedValues(HttpHeaderContants.HEADER_USER_ID_HASHED);
            string userHashedId = null;
            if (userIdentityIds != null && userIdentityIds.Any())
            {
                userHashedId = userIdentityIds.FirstOrDefault();
            }

            var contextHeaders = new SessionContextHeaders()
            {
                //AuthenticationToken = authenticationToken,
                UserIdentityId = userHashedId
            };

            return contextHeaders;
        }
    }
}
