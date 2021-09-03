using Microsoft.AspNetCore.Identity;
using System;

namespace Camino.Core.Domain.Identities
{
    public class ApplicationUserToken : IdentityUserToken<long>
    {
        public long Id { get; set; }
        public DateTimeOffset ExpiryTime { get; set; }
    }
}
