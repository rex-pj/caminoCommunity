using Coco.Common.Const;
using Coco.Common.Exceptions;
using Coco.Framework.SessionManager.Contracts;
using HotChocolate.Types;

namespace Coco.Framework.Infrastructure.Middlewares
{
    public class InitializeSessionDirectiveType : DirectiveType
    {
        protected override void Configure(IDirectiveTypeDescriptor descriptor)
        {
            descriptor.Name("InitializeSession");
            descriptor.Location(DirectiveLocation.Schema);
            descriptor.Location(DirectiveLocation.Object);
            descriptor.Location(DirectiveLocation.FieldDefinition);
            descriptor.Use(next => async context =>
            {
                var sessionContext = context.Service<ISessionContext>();
                if (sessionContext == null)
                {
                    throw new CocoApplicationException($"{SessionContextConst.SESSION_CONTEXT} is not registered");
                }

                context.ContextData[SessionContextConst.SESSION_CONTEXT] = sessionContext;
                if (!context.ContextData.ContainsKey(SessionContextConst.CURRENT_USER))
                {
                    var currentuser = await sessionContext.GetCurrentUserAsync();
                    if (currentuser != null && currentuser.Id > 0)
                    {
                        context.ContextData[SessionContextConst.CURRENT_USER] = currentuser;
                    }
                }

                await next.Invoke(context);
            });
        }
    }
}
