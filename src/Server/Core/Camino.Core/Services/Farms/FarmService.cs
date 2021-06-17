using Camino.Core.Contracts.Services.Farms;
using Camino.Shared.Results.Farms;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Requests.Farms;
using Camino.Core.Contracts.Repositories.Farms;
using System.Linq;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Shared.Results.Media;
using Camino.Shared.Enums;
using System;

namespace Camino.Services.Farms
{
    public class FarmService : IFarmService
    {
        private readonly IFarmRepository _farmRepository;
        private readonly IFarmPictureRepository _farmPictureRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserPhotoRepository _userPhotoRepository;

        public FarmService(IFarmRepository farmRepository, IFarmPictureRepository farmPictureRepository, IUserRepository userRepository,
            IUserPhotoRepository userPhotoRepository)
        {
            _farmRepository = farmRepository;
            _farmPictureRepository = farmPictureRepository;
            _userRepository = userRepository;
            _userPhotoRepository = userPhotoRepository;
        }

        public async Task<FarmResult> FindAsync(IdRequestFilter<long> filter)
        {
            var exist = await _farmRepository.FindAsync(filter);
            return exist;
        }

        public FarmResult FindByName(string name)
        {
            return _farmRepository.FindByName(name);
        }

        public async Task<FarmResult> FindDetailAsync(IdRequestFilter<long> filter)
        {
            var exist = await _farmRepository.FindDetailAsync(filter);
            if (exist == null)
            {
                return null;
            }

            var pictures = await _farmPictureRepository.GetFarmPicturesByFarmIdAsync(new IdRequestFilter<long> {
                Id = filter.Id
            });
            exist.Pictures = pictures.Select(x => new PictureResult
            {
                Id = x.PictureId
            });

            var createdByUserName = (await _userRepository.FindByIdAsync(exist.CreatedById)).DisplayName;
            exist.CreatedBy = createdByUserName;

            var updatedByUserName = (await _userRepository.FindByIdAsync(exist.UpdatedById)).DisplayName;
            exist.UpdatedBy = updatedByUserName;

            return exist;
        }

        public async Task<IList<FarmResult>> SelectAsync(SelectFilter filter, int page = 1, int pageSize = 10)
        {
            var famrs = await _farmRepository.SelectAsync(filter, page, pageSize);
            return famrs;
        }

        public async Task<BasePageList<FarmResult>> GetAsync(FarmFilter filter)
        {
            var famrsPageList = await _farmRepository.GetAsync(filter);
            var createdByIds = famrsPageList.Collections.Select(x => x.CreatedById).ToArray();
            var updatedByIds = famrsPageList.Collections.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetNameByIdsAsync(updatedByIds);

            var farmIds = famrsPageList.Collections.Select(x => x.Id);
            var pictureTypeId = (int)FarmPictureType.Thumbnail;
            var pictures = await _farmPictureRepository.GetFarmPicturesByFarmIdsAsync(farmIds, pictureTypeId);

            var userAvatars = await _userPhotoRepository.GetUserPhotosByUserIds(createdByIds, UserPhotoKind.Avatar);
            foreach (var farm in famrsPageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == farm.CreatedById);
                farm.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == farm.UpdatedById);
                farm.UpdatedBy = updatedBy.DisplayName;

                var farmPictures = pictures.Where(x => x.FarmId == farm.Id);
                if (farmPictures != null && farmPictures.Any())
                {
                    farm.Pictures = farmPictures.Select(x => new PictureResult
                    {
                        Id = x.PictureId
                    });
                }

                var avatar = userAvatars.FirstOrDefault(x => x.UserId == farm.CreatedById);
                if (avatar != null)
                {
                    farm.CreatedByPhotoCode = avatar.Code;
                }
            }

            return famrsPageList;
        }

        public async Task<long> CreateAsync(FarmModifyRequest request)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            request.CreatedDate = modifiedDate;
            request.UpdatedDate = modifiedDate;
            var id = await _farmRepository.CreateAsync(request);
            if (id > 0 && request.Pictures.Any())
            {
                await _farmPictureRepository.CreateAsync(new FarmPicturesModifyRequest
                {
                    CreatedById = request.CreatedById,
                    CreatedDate = request.CreatedDate,
                    FarmId = id,
                    Pictures = request.Pictures,
                    UpdatedById = request.UpdatedById,
                    UpdatedDate = request.UpdatedDate
                });
            }

            return id;
        }

        public async Task<bool> UpdateAsync(FarmModifyRequest request)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            request.UpdatedDate = modifiedDate;
            var isUpdated = await _farmRepository.UpdateAsync(request);
            if (isUpdated)
            {
                await _farmPictureRepository.UpdateAsync(new FarmPicturesModifyRequest
                {
                    FarmId = request.Id,
                    CreatedById = request.CreatedById,
                    UpdatedById = request.UpdatedById,
                    CreatedDate = modifiedDate,
                    UpdatedDate = modifiedDate,
                    Pictures = request.Pictures
                });
            }
            return isUpdated;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // Delete farm pictures
            await _farmPictureRepository.DeleteByFarmIdAsync(id);
            return await _farmRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(FarmModifyRequest request)
        {
            // Soft delete farm pictures
            await _farmPictureRepository.UpdateStatusByFarmIdAsync(new FarmPicturesModifyRequest
            {
                FarmId = request.Id,
                UpdatedById = request.UpdatedById
            }, PictureStatus.Deleted);
            return await _farmRepository.SoftDeleteAsync(request);
        }

        public async Task<bool> DeactivateAsync(FarmModifyRequest request)
        {
            await _farmPictureRepository.UpdateStatusByFarmIdAsync(new FarmPicturesModifyRequest
            {
                FarmId = request.Id,
                UpdatedById = request.UpdatedById
            }, PictureStatus.Inactived);
            return await _farmRepository.DeactivateAsync(request);
        }

        public async Task<bool> ActivateAsync(FarmModifyRequest request)
        {
            await _farmPictureRepository.UpdateStatusByFarmIdAsync(new FarmPicturesModifyRequest
            {
                FarmId = request.Id,
                UpdatedById = request.UpdatedById
            }, PictureStatus.Actived);

            return await _farmRepository.ActiveAsync(request);
        }

        public async Task<BasePageList<FarmPictureResult>> GetPicturesAsync(FarmPictureFilter filter)
        {
            var farmPicturePageList = await _farmPictureRepository.GetAsync(filter);

            var createdByIds = farmPicturePageList.Collections.GroupBy(x => x.PictureCreatedById).Select(x => x.Key);
            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);

            foreach (var farmPicture in farmPicturePageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == farmPicture.PictureCreatedById);
                farmPicture.PictureCreatedBy = createdBy.DisplayName;
            }

            return farmPicturePageList;
        }
    }
}
