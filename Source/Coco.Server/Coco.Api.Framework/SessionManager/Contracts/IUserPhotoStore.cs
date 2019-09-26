using Coco.Api.Framework.Models;
using Coco.Entities.Enums;
using Coco.Entities.Model.General;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IUserPhotoStore<TUser> : IDisposable where TUser : class
    {
        Task<ApiResult> UpdateAvatarAsync(UpdateUserPhotoModel model, long userId, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateCoverAsync(UpdateUserPhotoModel model, long userId, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteUserPhotoAsync(long userId, UserPhotoTypeEnum userPhotoType, CancellationToken cancellationToken = default);
    }
}
