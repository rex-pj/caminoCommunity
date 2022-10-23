using Camino.Shared.Constants;
using System.Security.Claims;

namespace Camino.Infrastructure.GraphQL.Resolvers
{
    public abstract class BaseResolver
    {
        protected BaseResolver()
        {
        }

        protected long GetCurrentUserId(ClaimsPrincipal claimsPrincipal)
        {
            if (!claimsPrincipal.Claims.Any(x => x.Type == HttpHeades.UserIdClaimKey))
            {
                return -1;
            }

            var claimValue = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == HttpHeades.UserIdClaimKey).Value;
            if (string.IsNullOrEmpty(claimValue))
            {
                return -1;
            }

            return long.Parse(claimValue);
        }
    }
}
