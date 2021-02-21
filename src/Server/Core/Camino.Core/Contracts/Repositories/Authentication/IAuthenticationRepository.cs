using Camino.Shared.Results.Identifiers;
using Camino.Shared.Requests.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Results.Authorization;

namespace Camino.Core.Contracts.Repositories.Authentication
{
    public interface IAuthenticationRepository
    {
        Task<UserResult> UpdatePasswordAsync(UserPasswordUpdateRequest model);
        IEnumerable<UserRoleResult> GetUserRoles(long userd);
    }
}
