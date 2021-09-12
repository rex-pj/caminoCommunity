using Camino.Infrastructure.Commons.Constants;
using System.Linq;
using System.Security.Claims;

namespace Camino.Framework.GraphQL.Resolvers
{
    public abstract class BaseResolver
    {
        protected BaseResolver()
        {
        }

        protected long GetCurrentUserId(ClaimsPrincipal claimsPrincipal)
        {
            if (!claimsPrincipal.Claims.Any(x => x.Type == HttpHeaderContants.UserIdClaimKey))
            {
                return -1;
            }

            var claimValue = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == HttpHeaderContants.UserIdClaimKey).Value;
            if (string.IsNullOrEmpty(claimValue))
            {
                return -1;
            }

            return long.Parse(claimValue);
        }
    }
}
