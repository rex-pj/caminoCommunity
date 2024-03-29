﻿using Camino.Shared.Enums;
using Camino.Core.Domains.Media;

namespace Camino.Core.Contracts.Repositories.Users
{
    public interface IUserPhotoRepository
    {
        Task DeleteAsync(UserPhoto userPhoto, bool needSaveChanges = false);
        Task<long> CreateAsync(UserPhoto userPhoto, bool needSaveChanges = false);
        Task DeleteByUserIdAsync(long userId, UserPictureTypes userPhotoType);
        Task<UserPhoto> GetByIdAsync(long id, UserPictureTypes type);
        Task<UserPhoto> GetByUserIdAsync(long userId, UserPictureTypes typeId);
        Task<long> GetIdByUserIdAsync(long userId, UserPictureTypes typeId);
        Task<IEnumerable<UserPhoto>> GetListAsync(long userId);
        Task<IList<UserPhoto>> GetListByUserIdsAsync(IEnumerable<long> userIds, UserPictureTypes typeId);
        Task UpdateAsync(UserPhoto userPhoto);
    }
}
