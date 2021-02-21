using Camino.Shared.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Results.Media;
using Camino.Shared.Requests.Identifiers;

namespace Camino.Core.Contracts.Services.Users
{
    public interface IUserPhotoService
    {
        Task<UserPhotoUpdateRequest> UpdateUserPhotoAsync(UserPhotoUpdateRequest request, long userId);
        Task DeleteUserPhotoAsync(long userId, UserPhotoKind userPhotoType);
        Task<UserPhotoResult> GetUserPhotoByCodeAsync(string code, UserPhotoKind typeId);
        UserPhotoResult GetUserPhotoByUserId(long userId, UserPhotoKind typeId);
        Task<IEnumerable<UserPhotoResult>> GetUserPhotosAsync(long userId);
        IList<UserPhotoResult> GetUserPhotoByUserIds(IEnumerable<long> userIds, UserPhotoKind typeId);
    }
}
