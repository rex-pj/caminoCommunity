using Coco.Api.Framework.SessionManager.Contracts;
using HotChocolate.Types;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Api.Framework.Middlewares
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
                if (sessionContext == null || sessionContext.CurrentUser == null)
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
