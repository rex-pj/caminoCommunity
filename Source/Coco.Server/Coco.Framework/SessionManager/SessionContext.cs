using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.Commons.Constants;
using Coco.Framework.Models;
using Microsoft.AspNetCore.Http;

namespace Coco.Framework.SessionManager
{
    public class SessionContext : ISessionContext
    {
        public readonly HttpContext HttpContext;
        public readonly HttpRequest HttpRequest;
        public readonly IHeaderDictionary RequestHeaders;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManager<ApplicationUser> _userManager;
        public string AuthenticationToken { get; protected set; }
        protected SessionContextHeaders AuthorizationHeaders { get; set; }

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

            AuthorizationHeaders = GetAuthorizationHeaders();
            if (AuthorizationHeaders != null)
            {
                AuthenticationToken = AuthorizationHeaders.AuthenticationToken;
            }

            CurrentUser = GetLoggedUser();
        }

        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        public ApplicationUser CurrentUser { get; set; }

        protected ApplicationUser GetLoggedUser()
        {
            if (!IsAuthorizationHeadersValid())
            {
                return new ApplicationUser();
            }

            var user = _userManager
                    .GetLoggingUser(AuthorizationHeaders.UserIdentityId, AuthorizationHeaders.AuthenticationToken);
            user.AuthenticationToken = AuthorizationHeaders.AuthenticationToken;
            user.UserIdentityId = AuthorizationHeaders.UserIdentityId;

            return user;
        }

        public SessionContextHeaders GetAuthorizationHeaders()
        {
            if (RequestHeaders == null)
            {
                return null;
            }

            var contextHeaders = new SessionContextHeaders()
            {
                AuthenticationToken = RequestHeaders[HttpHeaderContants.HEADER_AUTHORIZATION],
                UserIdentityId = RequestHeaders[HttpHeaderContants.HEADER_USER_ID_HASHED]
            };

            return contextHeaders;
        }

        private bool IsAuthorizationHeadersValid()
        {
            return AuthorizationHeaders != null
                && !string.IsNullOrEmpty(AuthorizationHeaders.AuthenticationToken)
                && !string.IsNullOrEmpty(AuthorizationHeaders.UserIdentityId);
        }
    }
}
