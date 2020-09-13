using Camino.Data.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Service.Projections.Content;
using Camino.Service.Projections.Request;

namespace Camino.Service.Business.Users.Contracts
{
    public interface IUserPhotoBusiness
    {
        Task<UserPhotoUpdateRequest> UpdateUserPhotoAsync(UserPhotoUpdateRequest model, long userId);
        Task DeleteUserPhotoAsync(long userId, UserPhotoKind userPhotoType);
        Task<UserPhotoProjection> GetUserPhotoByCodeAsync(string code, UserPhotoKind type);
        UserPhotoProjection GetUserPhotoByUserId(long userId, UserPhotoKind type);
        Task<IEnumerable<UserPhotoProjection>> GetUserPhotosAsync(long userId);
    }
}
