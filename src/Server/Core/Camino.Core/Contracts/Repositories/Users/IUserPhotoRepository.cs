using Camino.Shared.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Results.Media;
using Camino.Shared.Requests.Identifiers;

namespace Camino.Core.Contracts.Repositories.Users
{
    public interface IUserPhotoRepository
    {
        Task<UserPhotoUpdateRequest> UpdateUserPhotoAsync(UserPhotoUpdateRequest request, long userId);
        Task DeleteUserPhotoAsync(long userId, UserPhotoKind userPhotoType);
        Task<UserPhotoResult> GetUserPhotoByCodeAsync(string code, UserPhotoKind type);
        UserPhotoResult GetUserPhotoByUserId(long userId, UserPhotoKind type);
        Task<IEnumerable<UserPhotoResult>> GetUserPhotosAsync(long userId);
        IList<UserPhotoResult> GetUserPhotoByUserIds(IEnumerable<long> userIds, UserPhotoKind typeId);
        Task<IList<UserPhotoResult>> GetUserPhotosByUserIds(IEnumerable<long> userIds, UserPhotoKind typeId);
        Task<string> GetCodeByUserId(long userId, UserPhotoKind typeId);
    }
}
