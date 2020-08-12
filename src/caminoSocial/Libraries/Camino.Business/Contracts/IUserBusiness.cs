using Camino.Business.Dtos.Identity;
using Camino.Business.Dtos.General;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Camino.Business.Contracts
{
    public interface IUserBusiness
    {
        Task<UserDto> CreateAsync(UserDto userDto);
        Task<UserDto> FindByEmailAsync(string email);
        Task<UserDto> FindByUsernameAsync(string username);
        Task DeleteAsync(long id);
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(UserIdentifierUpdateDto model);
        Task<UserDto> FindByIdAsync(long id);
        Task<UserFullDto> FindFullByIdAsync(long id);
        Task<UpdatePerItem> UpdateInfoItemAsync(UpdatePerItem model);
        Task<bool> ActiveAsync(long id);
        Task<UserDto> UpdateAsync(UserDto user);
        Task<PageListDto<UserFullDto>> GetAsync(UserFilterDto filter);
        List<UserFullDto> Search(string query = "", List<long> currentUserIds = null, int page = 1, int pageSize = 10);
    }
}
