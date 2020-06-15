using Coco.Entities.Dtos.User;
using Coco.Entities.Dtos.General;
using System.Threading.Tasks;
using System.Collections.Generic;
using Coco.Entities.Dtos.Auth;

namespace Coco.Business.Contracts
{
    public interface IUserBusiness
    {
        Task<UserDto> CreateAsync(UserDto user);
        UserDto GetLoggedIn(long id);
        Task<UserDto> FindByEmailAsync(string email);
        Task<UserDto> FindByUsernameAsync(string username);
        Task DeleteAsync(long id);
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(UserIdentifierUpdateDto model);
        Task<UserDto> FindByIdAsync(long id);
        Task<UserFullDto> FindFullByIdAsync(long id);
        Task<UpdatePerItemDto> UpdateInfoItemAsync(UpdatePerItemDto model);
        Task<UserDto> UpdatePasswordAsync(UserPasswordUpdateDto model);
        Task<bool> ActiveAsync(long id);
        Task<UserDto> UpdateAsync(UserDto user);
        List<UserFullDto> GetFull();
        List<UserFullDto> Search(string query = "", int page = 1, int pageSize = 10);
        UserRoleAuthorizationPoliciesDto GetUserRolesAuthorizationPolicies(UserDto user);
    }
}
