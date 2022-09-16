using Microsoft.AspNetCore.Identity;

namespace Camino.Infrastructure.Identity.Core
{
    public class ApplicationUserToken : IdentityUserToken<long>
    {
        public long Id { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
