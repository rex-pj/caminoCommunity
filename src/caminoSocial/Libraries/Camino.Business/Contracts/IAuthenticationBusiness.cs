using Camino.Business.Dtos.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Business.Contracts
{
    public interface IAuthenticationBusiness
    {
        Task<UserDto> UpdatePasswordAsync(UserPasswordUpdateDto model);
        IEnumerable<UserRoleDto> GetUserRoles(long userd);
        UserDto GetLoggedIn(long id);
    }
}
