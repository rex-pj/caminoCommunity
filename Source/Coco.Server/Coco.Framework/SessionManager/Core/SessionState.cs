using System.Collections.Concurrent;

namespace Coco.Framework.SessionManager.Core
{
    public class SessionState
    {
        public SessionState()
        {
            Sessions = new ConcurrentDictionary<string, object>();
        }

        public ConcurrentDictionary<string, object> Sessions { get; set; }
    }
}
