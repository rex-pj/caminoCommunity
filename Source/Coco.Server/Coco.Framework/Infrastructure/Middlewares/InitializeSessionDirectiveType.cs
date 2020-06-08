using Coco.Common.Const;
using Coco.Common.Exceptions;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.SessionManager.Core;
using HotChocolate.Types;
using System.Collections.Concurrent;
using System.Threading.Tasks;

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

                var sessionState = context.Service<SessionState>();
                sessionState.Sessions[SessionContextConst.SESSION_CONTEXT] = sessionContext;
                if (!sessionState.Sessions.ContainsKey(SessionContextConst.CURRENT_USER))
                {
                    sessionState.Sessions[SessionContextConst.CURRENT_USER] = sessionContext.GetCurrentUserAsync();
                }

                if (!context.ContextData.ContainsKey(SessionContextConst.CURRENT_USER) && sessionState.Sessions.ContainsKey(SessionContextConst.CURRENT_USER))
                {
                    var currentUser = sessionState.Sessions[SessionContextConst.CURRENT_USER] as Task<ApplicationUser>;
                    context.ContextData[SessionContextConst.CURRENT_USER] = await currentUser;
                }

                await next.Invoke(context);
            });
        }
    }
}
