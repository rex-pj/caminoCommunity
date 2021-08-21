using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Camino.Core.Contracts.Helpers;
using Camino.Infrastructure.Commons.Constants;

namespace Camino.Framework.GraphQL
{
    public class GraphQlRequestInterceptor : DefaultHttpRequestInterceptor
    {
        public override async ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor,
            IQueryRequestBuilder requestBuilder, CancellationToken cancellationToken)
        {
            var token = context.Request.Headers[HttpHeaderContants.HEADER_AUTHORIZATION];
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            var jwtHelper = context.RequestServices.GetRequiredService<IJwtHelper>();
            var claimsIdentity = await jwtHelper.ValidateTokenAsync(token);
            if (claimsIdentity.IsAuthenticated)
            {
                context.User.AddIdentity(claimsIdentity);
            }

            await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
        }
    }
}
