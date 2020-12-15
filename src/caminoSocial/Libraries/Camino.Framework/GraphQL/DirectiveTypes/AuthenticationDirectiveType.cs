using Camino.Core.Constants;
using Camino.Core.Exceptions;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Models;
using HotChocolate.Types;
using Microsoft.AspNetCore.Mvc;

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
                if (!context.ContextData.ContainsKey(SessionContextConst.CURRENT_USER))
                {
                    context.Result = new ForbidResult();
                }

                var applicationUser = context.ContextData[SessionContextConst.CURRENT_USER] as ApplicationUser;
                if (applicationUser == null || applicationUser.Id <= 0)
                {
                    context.Result = new ForbidResult();
                }

                await next.Invoke(context);
            });
        }
    }
}
