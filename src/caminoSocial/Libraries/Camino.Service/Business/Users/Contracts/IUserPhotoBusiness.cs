using Camino.Data.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Service.Data.Content;
using Camino.Service.Data.Request;

namespace Camino.Service.Business.Users.Contracts
{
    public interface IUserPhotoBusiness
    {
        Task<UserPhotoUpdateRequest> UpdateUserPhotoAsync(UserPhotoUpdateRequest model, long userId);
        Task DeleteUserPhotoAsync(long userId, UserPhotoKind userPhotoType);
        Task<UserPhotoResult> GetUserPhotoByCodeAsync(string code, UserPhotoKind type);
        UserPhotoResult GetUserPhotoByUserId(long userId, UserPhotoKind type);
        Task<IEnumerable<UserPhotoResult>> GetUserPhotosAsync(long userId);
    }
}
