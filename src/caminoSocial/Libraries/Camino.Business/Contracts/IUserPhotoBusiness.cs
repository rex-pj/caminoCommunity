using Camino.Data.Enums;
using Camino.Business.Dtos.General;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Business.Dtos.Content;

namespace Camino.Business.Contracts
{
    public interface IUserPhotoBusiness
    {
        Task<UserPhotoUpdation> UpdateUserPhotoAsync(UserPhotoUpdation model, long userId);
        Task DeleteUserPhotoAsync(long userId, UserPhotoKind userPhotoType);
        Task<UserPhotoDto> GetUserPhotoByCodeAsync(string code, UserPhotoKind type);
        UserPhotoDto GetUserPhotoByUserId(long userId, UserPhotoKind type);
        Task<IEnumerable<UserPhotoDto>> GetUserPhotosAsync(long userId);
    }
}
