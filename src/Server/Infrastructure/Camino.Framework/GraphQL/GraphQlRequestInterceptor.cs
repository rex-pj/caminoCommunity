using Camino.Core.Constants;
using Camino.Core.Contracts.IdentityManager;
using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Framework.GraphQL
{
    public class GraphQlRequestInterceptor : DefaultHttpRequestInterceptor
    {
        public override async ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor,
            IQueryRequestBuilder requestBuilder, CancellationToken cancellationToken)
        {
            var sessionContext = context.RequestServices.GetService<ISessionContext>();
            var applicationUser = await sessionContext.GetCurrentUserAsync();
            if (applicationUser != null)
            {
                requestBuilder.TryAddProperty(SessionContextConst.CURRENT_USER, applicationUser);
            }

            await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
        }
    }
}
