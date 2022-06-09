using Camino.Core.Domains.Users;

namespace Camino.Core.Domains.Authentication
{
    public class UserToken
    {
        public long Id { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public long UserId { get; set; }
        public string Value { get; set; }
        public DateTimeOffset ExpiryTime { get; set; }
        public virtual User User { get; set; }
    }
}
