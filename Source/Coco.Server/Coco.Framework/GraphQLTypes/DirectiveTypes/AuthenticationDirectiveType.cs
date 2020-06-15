using Coco.Common.Const;
using Coco.Common.Exceptions;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.SessionManager.Core;
using HotChocolate.Types;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Coco.Framework.GraphQLTypes.DirectiveTypes
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

                if(sessionState.CurrentUser == null || sessionState.CurrentUser.Id <= 0)
                {
                    context.Result = new ForbidResult();
                }

                await next.Invoke(context);
            });
        }
    }
}
