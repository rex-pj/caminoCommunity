using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Constants;
using Camino.Shared.Exceptions;
using Camino.Infrastructure.Identity.Core;

namespace Camino.Infrastructure.GraphQL
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

            var userManager = context.RequestServices.GetRequiredService<IUserManager<ApplicationUser>>();
            try
            {
                var claimsIdentity = await userManager.ValidateTokenAsync(token);
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
