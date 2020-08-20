using Camino.Service.Data.Identity;
using Camino.Service.Data.Filters;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Service.Data.Request;
using Camino.Service.Data.Page;

namespace Camino.Service.Business.Users.Contracts
{
    public interface IUserBusiness
    {
        Task<UserResult> CreateAsync(UserResult userDto);
        Task<UserResult> FindByEmailAsync(string email);
        Task<UserResult> FindByUsernameAsync(string username);
        Task DeleteAsync(long id);
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(UserIdentifierUpdateDto model);
        Task<UserResult> FindByIdAsync(long id);
        Task<UserFullDto> FindFullByIdAsync(long id);
        Task<UpdateItemRequest> UpdateInfoItemAsync(UpdateItemRequest model);
        Task<bool> ActiveAsync(long id);
        Task<UserResult> UpdateAsync(UserResult user);
        Task<PageList<UserFullDto>> GetAsync(UserFilter filter);
        List<UserFullDto> Search(string query = "", List<long> currentUserIds = null, int page = 1, int pageSize = 10);
    }
}
