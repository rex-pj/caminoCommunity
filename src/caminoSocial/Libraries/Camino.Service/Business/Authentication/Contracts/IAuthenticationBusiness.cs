using Camino.Service.Data.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authentication.Contracts
{
    public interface IAuthenticationBusiness
    {
        Task<UserResult> UpdatePasswordAsync(UserPasswordUpdateDto model);
        IEnumerable<UserRoleResult> GetUserRoles(long userd);
        UserResult GetLoggedIn(long id);
    }
}
