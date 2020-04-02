using Coco.Api.Framework.Models;
using Coco.Entities.Enums;
using Coco.Entities.Dtos.General;
using System;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IUserPhotoStore<TUser> : IDisposable where TUser : class
    {
        Task<UserPhotoUpdateDto> UpdateAvatarAsync(UserPhotoUpdateDto model, long userId);
        Task<UserPhotoUpdateDto> UpdateCoverAsync(UserPhotoUpdateDto model, long userId);
        Task<UserPhotoUpdateDto> DeleteUserPhotoAsync(long userId, UserPhotoTypeEnum userPhotoType);
    }
}
