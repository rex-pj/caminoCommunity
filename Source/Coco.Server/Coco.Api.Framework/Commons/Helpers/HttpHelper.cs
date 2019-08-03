using Coco.Api.Framework.Commons.Constants;
using Coco.Api.Framework.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.Linq;

namespace Coco.Api.Framework.Commons.Helpers
{
    public static class HttpHelper
    {
        public static SessionContextHeaders GetAuthorizationHeaders(HttpContext context)
        {
            var httpContext = context as DefaultHttpContext;
            var httpHeaders = httpContext.Request.Headers as HttpRequestHeaders;
            
            var authenticationToken = httpHeaders.HeaderAuthorization;
            var userIdentityIds = httpHeaders.GetCommaSeparatedValues(HttpHeaderContants.HEADER_USER_ID_HASHED);
            var userHashedId = userIdentityIds.FirstOrDefault();

            var headerParams = new SessionContextHeaders()
            {
                AuthenticationToken = authenticationToken,
                UserIdentityId = userHashedId
            };

            return headerParams;
        }
    }
}
