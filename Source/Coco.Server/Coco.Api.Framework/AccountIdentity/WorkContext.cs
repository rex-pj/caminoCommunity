using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.Commons.Constants;
using Coco.Api.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity
{
    public class WorkContext : IWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountManager<ApplicationUser> _accountManager;
        public readonly HttpContext HttpContext;
        public readonly HttpRequest HttpRequest;
        public readonly IHeaderDictionary RequestHeaders;
        public string AuthenticationToken { get; protected set; }

        public WorkContext(IHttpContextAccessor httpContextAccessor, IAccountManager<ApplicationUser> accountManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _accountManager = accountManager;
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

            var user = new ApplicationUser();
            if (headerParams != null
                && !string.IsNullOrEmpty(headerParams.AuthenticationToken)
                && !string.IsNullOrEmpty(headerParams.UserIdentityId))
            {
                user = _accountManager
                    .GetLoggingUser(headerParams.UserIdentityId, headerParams.AuthenticationToken);

                AuthenticationToken = headerParams.AuthenticationToken;
                user.UserIdentityId = headerParams.UserIdentityId;
            }

            return user;
        }

        public WorkContextHeaders GetAuthorizationHeaders()
        {
            var httpHeaders = RequestHeaders as HttpRequestHeaders;

            if (httpHeaders == null)
            {
                return null;
            }

            var authenticationToken = httpHeaders.HeaderAuthorization;

            var userIdentityIds = httpHeaders.GetCommaSeparatedValues(HttpHeaderContants.HEADER_USER_ID_HASHED);
            string userHashedId = null;
            if (userIdentityIds != null && userIdentityIds.Any())
            {
                userHashedId = userIdentityIds.FirstOrDefault();
            }

            var contextHeaders = new WorkContextHeaders()
            {
                AuthenticationToken = authenticationToken,
                UserIdentityId = userHashedId
            };

            return contextHeaders;
        }
    }
}
