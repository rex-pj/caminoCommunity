using Camino.Core.Constants;
using Camino.IdentityManager.Contracts;
using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Camino.Framework.GraphQL
{
    public class GraphQlRequestInterceptor : DefaultHttpRequestInterceptor
    {
        public override ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor,
            IQueryRequestBuilder requestBuilder, CancellationToken cancellationToken)
        {
            var sessionContext = context.RequestServices.GetService(typeof(ISessionContext)) as ISessionContext;
            var sessionTask = sessionContext.GetCurrentUserAsync();
            var applicationUser = sessionTask.GetAwaiter().GetResult();
            if (applicationUser != null)
            {
                requestBuilder.TryAddProperty(SessionContextConst.CURRENT_USER, applicationUser);
            }

            return base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
        }
    }
}
