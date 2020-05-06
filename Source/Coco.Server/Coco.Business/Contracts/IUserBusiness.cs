using Coco.Entities.Dtos.User;
using Coco.Entities.Dtos.General;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Coco.Business.Contracts
{
    public interface IUserBusiness
    {
        Task<UserDto> CreateAsync(UserDto user);
        UserDto GetLoggedIn(long id);
        Task<UserDto> FindUserByEmail(string email);
        Task<UserDto> FindUserByUsername(string username);
        void Delete(long id);
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(UserIdentifierUpdateDto model);
        Task<UserDto> FindByIdAsync(long id);
        Task<UserFullDto> FindFullByIdAsync(long id);
        Task<UpdatePerItemDto> UpdateInfoItemAsync(UpdatePerItemDto model);
        Task<UserDto> UpdatePasswordAsync(UserPasswordUpdateDto model);
        Task<bool> ActiveAsync(long id);
        List<UserFullDto> GetFull();
    }
}
