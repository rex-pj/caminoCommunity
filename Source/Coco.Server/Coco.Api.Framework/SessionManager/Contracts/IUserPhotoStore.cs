using Coco.Api.Framework.Models;
using Coco.Entities.Enums;
using Coco.Entities.Dtos.General;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IUserPhotoStore<TUser> : IDisposable where TUser : class
    {
        Task<ApiResult> UpdateAvatarAsync(UpdateUserPhotoDto model, long userId);
        Task<ApiResult> UpdateCoverAsync(UpdateUserPhotoDto model, long userId);
        Task<ApiResult> DeleteUserPhotoAsync(long userId, UserPhotoTypeEnum userPhotoType);
    }
}
