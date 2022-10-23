using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;
using Camino.Shared.Constants;
using Camino.Shared.Exceptions;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Infrastructure.Identity.Core;

namespace Camino.Infrastructure.GraphQL.DirectiveTypes
{
    public class AuthenticationDirectiveType : DirectiveType
    {
        protected override void Configure(IDirectiveTypeDescriptor descriptor)
        {
            descriptor.Name("Authentication");
            descriptor.Location(DirectiveLocation.Query);
            descriptor.Location(DirectiveLocation.Mutation);
            descriptor.Location(DirectiveLocation.FieldDefinition);
            descriptor.Use(next => async context =>
            {
                var httpContextAccessor = context.Services.GetService<IHttpContextAccessor>();
                var token = httpContextAccessor.HttpContext.Request.Headers[HttpHeades.HeaderAuthenticationAccessToken];
                if (string.IsNullOrEmpty(token))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                try
                {
                    var userManager = context.Services.GetRequiredService<IUserManager<ApplicationUser>>();
                    var claimsIdentity = await userManager.ValidateTokenAsync(token);
                    if (!claimsIdentity.IsAuthenticated)
                    {
                        context.Result = new ForbidResult();
                    }
                }
                catch (CaminoAuthenticationException)
                {
                    context.ReportError(new Error(HttpStatusCode.Forbidden.ToString(), StatusCodes.Status403Forbidden.ToString()));
                    context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                    return;
                }
                catch (Exception)
                {
                    context.ReportError(new Error(HttpStatusCode.InternalServerError.ToString(), StatusCodes.Status500InternalServerError.ToString()));
                    context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    return;
                }

                await next.Invoke(context);
            });
        }
    }
}
