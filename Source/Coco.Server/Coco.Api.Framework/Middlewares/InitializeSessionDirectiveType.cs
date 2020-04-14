using Coco.Api.Framework.SessionManager.Contracts;
using HotChocolate.Types;

namespace Coco.Api.Framework.Middlewares
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
                if (sessionContext != null && sessionContext.CurrentUser != null)
                {
                    context.ContextData["SessionContext"] = sessionContext;
                }

                await next.Invoke(context);
            });
        }
    }
}
