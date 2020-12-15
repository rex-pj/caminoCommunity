using Camino.IdentityManager.Contracts;

namespace Camino.Framework.GraphQL.Resolvers
{
    public abstract class BaseResolver
    {
        protected readonly ISessionContext _sessionContext;

        protected BaseResolver(ISessionContext sessionContext)
        {
            _sessionContext = sessionContext;
        }
    }
}
