using Coco.Common.Const;
using Coco.Framework.SessionManager.Contracts;
using HotChocolate.Resolvers;
using System.Threading.Tasks;

namespace Coco.Framework.Infrastructure.Middlewares
{
    public class InitializeSessionMiddleware
    {
        private readonly FieldDelegate _next;
        private readonly ISessionContext _sessionContext;

        public InitializeSessionMiddleware(FieldDelegate next, ISessionContext sessionContext)
        {
            _next = next;
            _sessionContext = sessionContext;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            context.ContextData[SessionContextConst.SESSION_CONTEXT] = _sessionContext;
            if (!context.ContextData.ContainsKey(SessionContextConst.CURRENT_USER))
            {
                var currentuser = await _sessionContext.GetCurrentUserAsync();
                if (currentuser != null && currentuser.Id > 0)
                {
                    context.ContextData[SessionContextConst.CURRENT_USER] = currentuser;
                }
            }

            await _next.Invoke(context);
        }
    }
}
