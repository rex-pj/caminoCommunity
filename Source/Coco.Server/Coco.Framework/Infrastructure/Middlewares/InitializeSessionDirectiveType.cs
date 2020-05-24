using Coco.Common.Exceptions;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using HotChocolate.Types;

namespace Coco.Framework.Infrastructure.Middlewares
{
    public class InitializeSessionDirectiveType : DirectiveType
    {
        protected override void Configure(IDirectiveTypeDescriptor descriptor)
        {
            descriptor.Name("InitializeSession");
            descriptor.Location(DirectiveLocation.Mutation);
            descriptor.Location(DirectiveLocation.Query);
            descriptor.Location(DirectiveLocation.FieldDefinition);
            descriptor.Use(next => async context =>
            {
                var sessionContext = context.Service<ISessionContext>();
                if (sessionContext == null)
                {
                    throw new CocoApplicationException("SessionContext is not registered");
                }

                var currentUser = await sessionContext.GetLoggedUserAsync();
                if (currentUser != null && currentUser.Id > 0)
                {
                    context.ContextData["SessionContext"] = sessionContext;
                }

                await next.Invoke(context);
            });
        }
    }
}
