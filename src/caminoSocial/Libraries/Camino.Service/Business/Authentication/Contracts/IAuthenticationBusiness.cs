using Camino.Service.Projections.Identity;
using Camino.Service.Projections.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authentication.Contracts
{
    public interface IAuthenticationBusiness
    {
        Task<UserProjection> UpdatePasswordAsync(UserPasswordUpdateRequest model);
        IEnumerable<UserRoleProjection> GetUserRoles(long userd);
        UserProjection GetLoggedIn(long id);
    }
}
