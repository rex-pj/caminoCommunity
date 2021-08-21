using Camino.Core.Contracts.Helpers;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Camino.Infrastructure.Commons.Constants;

namespace Camino.Framework.GraphQL.DirectiveTypes
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
                var token = httpContextAccessor.HttpContext.Request.Headers[HttpHeaderContants.HEADER_AUTHORIZATION];
                if (string.IsNullOrEmpty(token))
                {
                    context.Result = new ForbidResult();
                }

                var jwtHelper = context.Services.GetService<IJwtHelper>();
                var claimsIdentity = await jwtHelper.ValidateTokenAsync(token);
                if (!claimsIdentity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                }

                await next.Invoke(context);
            });
        }
    }
}
