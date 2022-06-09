using Camino.Core.Domains.Users;

namespace Camino.Core.Domains.Authentication
{
    public class UserLogin
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderKey { get; set; }
        public virtual User User { get; set; }
    }
}
