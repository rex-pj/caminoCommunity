using Coco.Common.Const;
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
                    throw new CocoApplicationException($"{SessionContextConst.SESSION_CONTEXT} is not registered");
                }

                if (!context.ContextData.ContainsKey(SessionContextConst.SESSION_CONTEXT))
                {
                    context.ContextData[SessionContextConst.SESSION_CONTEXT] = sessionContext;
                    if (!context.ContextData.ContainsKey(SessionContextConst.CURRENT_USER))
                    {
                        var currentuser = await sessionContext.GetCurrentUserAsync();
                        if (currentuser != null && currentuser.Id > 0)
                        {
                            context.ContextData[SessionContextConst.CURRENT_USER] = currentuser;
                        }
                        else
                        {
                            context.Result = new ForbidResult();
                        }
                    }
                }

                await next.Invoke(context);
            });
        }
    }
}
