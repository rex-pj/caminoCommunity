using Camino.IdentityManager.Models;
using System.Collections.Concurrent;

namespace Camino.IdentityManager.Contracts.Core
{
    public class SessionState
    {
        public SessionState()
        {
            Sessions = new ConcurrentDictionary<string, object>();
        }

        public ConcurrentDictionary<string, object> Sessions { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }
}
