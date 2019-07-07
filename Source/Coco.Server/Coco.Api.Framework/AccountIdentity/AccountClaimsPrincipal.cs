using System.Security.Claims;
using System.Security.Principal;

namespace Coco.Api.Framework.AccountIdentity
{
    public class AccountClaimsPrincipal : ClaimsPrincipal
    {
        public string UserIdHashed { get; set; }
        public AccountClaimsPrincipal(IIdentity identities) : base(identities)
        {
        }
        public AccountClaimsPrincipal(IPrincipal principal) : base(principal)
        {
        }

        /// <summary>
        /// IsInRole answers the question: does an identity this principal possesses
        /// contain a claim of type RoleClaimType where the value is '==' to the role.
        /// </summary>
        /// <param name="role">The role to check for.</param>
        /// <returns>'True' if a claim is found. Otherwise 'False'.</returns>
        /// <remarks>Each Identity has its own definition of the ClaimType that represents a role.</remarks>
        public override bool IsInRole(string role)
        {
            var user = this.Identity;

            return false;
        }

        public override void AddIdentity(ClaimsIdentity identity)
        {
            base.AddIdentity(identity);
        }
    }
}
