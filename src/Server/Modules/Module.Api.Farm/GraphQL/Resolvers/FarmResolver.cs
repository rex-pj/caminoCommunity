using Camino.Framework.GraphQL.Resolvers;
using Camino.Framework.Models;
using Camino.Core.Domain.Identities;
using Camino.Core.Contracts.Services.Farms;
using Camino.Shared.Results.Farms;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Media;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using Module.Api.Farm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.IdentityManager;
using Camino.Shared.General;
using Camino.Shared.Requests.Farms;
using Camino.Shared.Requests.Media;

namespace Module.Api.Farm.GraphQL.Resolvers
{
    public class FarmResolver : BaseResolver, IFarmResolver
    {
        private readonly IFarmService _farmService;
        private readonly IUserManager<ApplicationUser> _userManager;

        public FarmResolver(ISessionContext sessionContext, IFarmService farmService, IUserManager<ApplicationUser> userManager)
            : base(sessionContext)
        {
            _farmService = farmService;
            _userManager = userManager;
        }

        public async Task<IEnumerable<SelectOption>> SelectUserFarmsAsync(ApplicationUser currentUser, FarmSelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FarmSelectFilterModel();
            }

            var filter = new SelectFilter()
            {
                CreatedById = currentUser.Id,
                CurrentIds = criterias.CurrentIds,
                Search = criterias.Query
            };

            var farms = await _farmService.SelectAsync(filter);
            if (farms == null || !farms.Any())
            {
                return new List<SelectOption>();
            }

            var farmSeletions = farms
                .Select(x => new SelectOption
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return farmSeletions;
        }

        public async Task<FarmModel> CreateFarmAsync(ApplicationUser currentUser, FarmModel criterias)
        {
            var farm = new FarmModifyRequest()
            {
                CreatedById = currentUser.Id,
                UpdatedById = currentUser.Id,
                Description = criterias.Description,
                Name = criterias.Name,
                FarmTypeId = criterias.FarmTypeId,
                Address = criterias.Address,
                Pictures = criterias.Pictures.Select(x => new PictureRequest()
                {
                    Base64Data = x.Base64Data,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                }),
            };

            var id = await _farmService.CreateAsync(farm);
            criterias.Id = id;
            return criterias;
        }

        public async Task<FarmModel> UpdateFarmAsync(ApplicationUser currentUser, FarmModel criterias)
        {
            var exist = await _farmService.FindAsync(new IdRequestFilter<long>
            {
                Id = criterias.Id
            });
            if (exist == null)
            {
                throw new Exception("No article found");
            }

            if (currentUser.Id != exist.CreatedById)
            {
                throw new UnauthorizedAccessException();
            }

            var farm = new FarmModifyRequest()
            {
                Id = criterias.Id,
                CreatedById = currentUser.Id,
                UpdatedById = currentUser.Id,
                Description = criterias.Description,
                Name = criterias.Name,
                FarmTypeId = criterias.FarmTypeId,
                Address = criterias.Address,
            };

            if (criterias.Pictures != null && criterias.Pictures.Any())
            {
                farm.Pictures = criterias.Pictures.Select(x => new PictureRequest()
                {
                    Base64Data = x.Base64Data,
                    ContentType = x.ContentType,
                    FileName = x.FileName,
                    Id = x.PictureId
                });
            }

            await _farmService.UpdateAsync(farm);
            return criterias;
        }

        public async Task<FarmPageListModel> GetUserFarmsAsync(FarmFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FarmFilterModel();
            }

            if (string.IsNullOrEmpty(criterias.UserIdentityId))
            {
                return new FarmPageListModel(new List<FarmModel>())
                {
                    Filter = criterias
                };
            }

            var userId = await _userManager.DecryptUserIdAsync(criterias.UserIdentityId);
            var filterRequest = new FarmFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search,
                CreatedById = userId
            };

            try
            {
                var farmPageList = await _farmService.GetAsync(filterRequest);
                var farms = await MapFarmsResultToModelAsync(farmPageList.Collections);

                var farmPage = new FarmPageListModel(farms)
                {
                    Filter = criterias,
                    TotalPage = farmPageList.TotalPage,
                    TotalResult = farmPageList.TotalResult
                };

                return farmPage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FarmPageListModel> GetFarmsAsync(FarmFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FarmFilterModel();
            }

            var filterRequest = new FarmFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search
            };

            if (!string.IsNullOrEmpty(criterias.ExclusiveCreatedIdentityId))
            {
                filterRequest.ExclusiveCreatedById = await _userManager.DecryptUserIdAsync(criterias.ExclusiveCreatedIdentityId);
            }

            try
            {
                var farmPageList = await _farmService.GetAsync(filterRequest);
                var farms = await MapFarmsResultToModelAsync(farmPageList.Collections);

                var farmPage = new FarmPageListModel(farms)
                {
                    Filter = criterias,
                    TotalPage = farmPageList.TotalPage,
                    TotalResult = farmPageList.TotalResult
                };

                return farmPage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FarmModel> GetFarmAsync(FarmFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FarmFilterModel();
            }

            try
            {
                var farmResult = await _farmService.FindDetailAsync(new IdRequestFilter<long>
                {
                    Id = criterias.Id
                });
                var farm = await MapFarmResultToModelAsync(farmResult);
                return farm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteFarmAsync(ApplicationUser currentUser, FarmFilterModel criterias)
        {
            try
            {
                var exist = await _farmService.FindAsync(new IdRequestFilter<long>
                {
                    Id = criterias.Id
                });
                if (exist == null || currentUser.Id != exist.CreatedById)
                {
                    return false;
                }

                return await _farmService.SoftDeleteAsync(new FarmModifyRequest { 
                    UpdatedById = currentUser.Id,
                    Id = criterias.Id
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<FarmModel> MapFarmResultToModelAsync(FarmResult farmResult)
        {
            var farm = new FarmModel()
            {
                FarmTypeId = farmResult.FarmTypeId,
                FarmTypeName = farmResult.FarmTypeName,
                Description = farmResult.Description,
                Id = farmResult.Id,
                CreatedBy = farmResult.CreatedBy,
                CreatedById = farmResult.CreatedById,
                CreatedDate = farmResult.CreatedDate,
                Name = farmResult.Name,
                Address = farmResult.Address,
                CreatedByPhotoCode = farmResult.CreatedByPhotoCode,
                Pictures = farmResult.Pictures.Select(y => new PictureRequestModel()
                {
                    PictureId = y.Id
                }),
            };

            farm.CreatedByIdentityId = await _userManager.EncryptUserIdAsync(farm.CreatedById);

            return farm;
        }

        private async Task<IList<FarmModel>> MapFarmsResultToModelAsync(IEnumerable<FarmResult> farmResults)
        {
            var farms = farmResults.Select(x => new FarmModel()
            {
                FarmTypeId = x.FarmTypeId,
                FarmTypeName = x.FarmTypeName,
                Description = x.Description,
                Id = x.Id,
                CreatedBy = x.CreatedBy,
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                Name = x.Name,
                Address = x.Address,
                CreatedByPhotoCode = x.CreatedByPhotoCode,
                Pictures = x.Pictures.Select(y => new PictureRequestModel()
                {
                    PictureId = y.Id
                }),
            }).ToList();

            foreach (var farm in farms)
            {
                farm.CreatedByIdentityId = await _userManager.EncryptUserIdAsync(farm.CreatedById);
                if (farm.Description.Length >= 150)
                {
                    farm.Description = $"{farm.Description.Substring(0, 150)}...";
                }
            }

            return farms;
        }
    }
}
