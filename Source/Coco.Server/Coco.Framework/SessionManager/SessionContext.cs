using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.Models;
using Microsoft.AspNetCore.Http;
using Coco.Common.Const;
using System.Threading.Tasks;

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
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            if (!IsAuthorizationHeadersValid())
            {
                return new ApplicationUser();
            }

            var userId = await _userManager.DecryptUserIdAsync(AuthorizationHeaders.UserIdentityId);
            var currentUser = await _userManager.FindByIdAsync(userId.ToString());
            if (currentUser == null)
            {
                return new ApplicationUser();
            }

            var user = await _userManager.FindByLoginAsync(ServiceProvidersNameConst.COCO_API_AUTH, AuthenticationToken);
            if(user == null || user.Id != currentUser.Id)
            {
                return new ApplicationUser();
            }
            
            user.Id = userId;
            UpdateAuthenticate(user);
            return user;
        }

        private void UpdateAuthenticate(ApplicationUser user)
        {
            user.AuthenticationToken = AuthorizationHeaders.AuthenticationToken;
            user.UserIdentityId = AuthorizationHeaders.UserIdentityId;
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
            return AuthorizationHeaders != null && !string.IsNullOrEmpty(AuthorizationHeaders.AuthenticationToken) && !string.IsNullOrEmpty(AuthorizationHeaders.UserIdentityId);
        }
    }
}
