using Coco.Common.Exceptions;
using Coco.Framework.SessionManager.Contracts;
using HotChocolate.Types;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Framework.Infrastructure.Middlewares
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
                var sessionContext = context.Service<ISessionContext>();
                if (sessionContext == null)
                {
                    throw new CocoApplicationException("SessionContext is not registered");
                }

                var currentUser = await sessionContext.GetCurrentUserAsync();
                if (currentUser == null || currentUser.Id <= 0)
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    context.ContextData["SessionContext"] = sessionContext;
                }

                await next.Invoke(context);
            });
        }
    }
}
