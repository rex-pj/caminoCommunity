using Coco.Api.Framework.Commons.Constants;
using Coco.Api.Framework.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.Linq;

namespace Coco.Api.Framework.Commons.Helpers
{
    public class HttpHelper
    {
        public static UserAuthenticationHeaders GetAuthorizationHeaders(ResolveFieldContext<object> context)
        {
            var httpContext = context.UserContext as DefaultHttpContext;
            var httpHeaders = httpContext.Request.Headers as HttpRequestHeaders;

            var authenticationToken = httpHeaders.HeaderAuthorization;
            var userHashedIds = httpHeaders.GetCommaSeparatedValues(HttpHeaderContants.HEADER_USER_ID_HASHED);
            var userHashedId = userHashedIds.FirstOrDefault();

            var headerParams = new UserAuthenticationHeaders()
            {
                AuthenticationToken = authenticationToken,
                UserIdHashed = userHashedId
            };

            return headerParams;
        }
    }
}
