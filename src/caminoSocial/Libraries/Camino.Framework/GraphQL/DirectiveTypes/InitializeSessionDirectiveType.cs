using Camino.Core.Constants;
using Camino.Core.Exceptions;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Contracts.Core;
using Camino.IdentityManager.Models;
using HotChocolate.Types;
using System.Threading.Tasks;

namespace Camino.Framework.GraphQL.DirectiveTypes
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
                    throw new CaminoApplicationException($"{SessionContextConst.SESSION_CONTEXT} is not registered");
                }

                var sessionState = context.Service<SessionState>();
                sessionState.Sessions[SessionContextConst.SESSION_CONTEXT] = sessionContext;
                if (!sessionState.Sessions.ContainsKey(SessionContextConst.CURRENT_USER))
                {
                    sessionState.Sessions[SessionContextConst.CURRENT_USER] = sessionContext.GetCurrentUserAsync();
                }

                if (!context.ContextData.ContainsKey(SessionContextConst.CURRENT_USER) && sessionState.Sessions.ContainsKey(SessionContextConst.CURRENT_USER))
                {
                    var currentUser = await (sessionState.Sessions[SessionContextConst.CURRENT_USER] as Task<ApplicationUser>);
                    sessionState.CurrentUser = currentUser;
                    context.ContextData[SessionContextConst.CURRENT_USER] = currentUser;
                }

                await next.Invoke(context);
            });
        }
    }
}
