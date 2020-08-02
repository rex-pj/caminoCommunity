using Camino.IdentityManager.Contracts.Core;
using Camino.IdentityManager.Models;

namespace Camino.Framework.GraphQL.Resolvers
{
    public abstract class BaseResolver
    {
        protected readonly ApplicationUser CurrentUser;

        protected BaseResolver(SessionState sessionState)
        {
            CurrentUser = sessionState.CurrentUser;
        }
    }
}
