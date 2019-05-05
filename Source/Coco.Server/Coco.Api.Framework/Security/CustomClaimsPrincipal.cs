using Coco.Business.Contracts;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Coco.Api.Framework.Security
{
    public class CustomClaimsPrincipal : ClaimsPrincipal
    {
        private readonly IRoleBusiness _roleBusiness;
        public CustomClaimsPrincipal(IIdentity identities, IRoleBusiness roleBusiness) : base(identities) {
            _roleBusiness = roleBusiness;
        }
        public CustomClaimsPrincipal(IPrincipal principal, IRoleBusiness roleBusiness) : base(principal) {
            _roleBusiness = roleBusiness;
        }

        private readonly List<ClaimsIdentity> _identities = new List<ClaimsIdentity>();
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
    }
}
