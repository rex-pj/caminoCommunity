using Camino.Core.Domain.Identities;
using Microsoft.AspNetCore.Http;
using Camino.Core.Constants;
using System.Threading.Tasks;
using Camino.Core.Contracts.IdentityManager;

namespace Camino.IdentityManager
{
    public class SessionContext : ISessionContext
    {
        public readonly HttpContext HttpContext;
        public readonly HttpRequest HttpRequest;
        public readonly IHeaderDictionary RequestHeaders;
        private readonly IUserManager<ApplicationUser> _userManager;
        public string AuthenticationToken { get; protected set; }
        protected SessionContextHeaders AuthorizationHeaders { get; set; }

        public SessionContext(IHttpContextAccessor httpContextAccessor, IUserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            HttpContext = httpContextAccessor.HttpContext;

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

        public async ValueTask<ApplicationUser> GetCurrentUserAsync()
        {
            if (!IsAuthorizationHeadersValid())
            {
                return new ApplicationUser();
            }

            var userId = await _userManager.DecryptUserIdAsync(AuthorizationHeaders.UserIdentityId);
            var user = await _userManager.FindByLoginAsync(ServiceProvidersNameConst.CAMINO_API_AUTH, AuthenticationToken);
            if(user == null || user.Id != userId)
            {
                return new ApplicationUser();
            }

            UpdateAuthenticate(user);
            return user;
        }

        public async ValueTask<bool> IsAuthenticatedAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            return currentUser != null && currentUser.Id > 0;
        }

        private void UpdateAuthenticate(ApplicationUser user)
        {
            user.AuthenticationToken = AuthorizationHeaders.AuthenticationToken;
            user.UserIdentityId = AuthorizationHeaders.UserIdentityId;
        }

        private SessionContextHeaders GetAuthorizationHeaders()
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
