using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Constants;
using Camino.Shared.Exceptions;

namespace Camino.Framework.GraphQL
{
    public class GraphQlRequestInterceptor : DefaultHttpRequestInterceptor
    {
        public override async ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor,
            IQueryRequestBuilder requestBuilder, CancellationToken cancellationToken)
        {
            var token = context.Request.Headers[HttpHeades.HeaderAuthenticationAccessToken];
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            var jwtHelper = context.RequestServices.GetRequiredService<IJwtHelper>();
            try
            {
                var claimsIdentity = await jwtHelper.ValidateTokenAsync(token);
                if (claimsIdentity.IsAuthenticated)
                {
                    context.User.AddIdentity(claimsIdentity);
                }
            }
            catch (CaminoAuthenticationException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            
            await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
        }
    }
}
