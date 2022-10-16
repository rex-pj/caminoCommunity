using Camino.Framework.GraphQL.Resolvers;
using Camino.Framework.Models;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using Module.Api.Farm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Camino.Application.Contracts.AppServices.Farms;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Configuration.Options;
using Camino.Infrastructure.Identity.Core;
using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Farms.Dtos;
using Camino.Application.Contracts.AppServices.Media.Dtos;

namespace Module.Api.Farm.GraphQL.Resolvers
{
    public class FarmResolver : BaseResolver, IFarmResolver
    {
        private readonly IFarmAppService _farmAppService;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public FarmResolver(IFarmAppService farmAppService, IUserManager<ApplicationUser> userManager,
            IOptions<PagerOptions> pagerOptions)
            : base()
        {
            _farmAppService = farmAppService;
            _userManager = userManager;
            _pagerOptions = pagerOptions.Value;
        }

        public async Task<IEnumerable<SelectOption>> SelectUserFarmsAsync(ClaimsPrincipal claimsPrincipal, FarmSelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FarmSelectFilterModel();
            }

            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var filter = new SelectFilter()
            {
                CreatedById = currentUserId,
                CurrentIds = criterias.CurrentIds,
                Keyword = criterias.Query
            };

            var farms = await _farmAppService.SelectAsync(filter, _defaultPageSelection, _pagerOptions.PageSize);
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

        public async Task<FarmIdResultModel> CreateFarmAsync(ClaimsPrincipal claimsPrincipal, CreateFarmModel criterias)
        {
            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var farm = new FarmModifyRequest
            {
                CreatedById = currentUserId,
                UpdatedById = currentUserId,
                Description = criterias.Description,
                Name = criterias.Name,
                FarmTypeId = criterias.FarmTypeId,
                Address = criterias.Address,
                Pictures = criterias.Pictures.Select(x => new PictureRequest
                {
                    Base64Data = x.Base64Data,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                }),
            };

            var id = await _farmAppService.CreateAsync(farm);
            return new FarmIdResultModel
            {
                Id = id
            };
        }

        public async Task<FarmIdResultModel> UpdateFarmAsync(ClaimsPrincipal claimsPrincipal, UpdateFarmModel criterias)
        {
            var exist = await _farmAppService.FindAsync(new IdRequestFilter<long>
            {
                Id = criterias.Id,
                CanGetInactived = true
            });

            if (exist == null)
            {
                throw new Exception("No article found");
            }

            var currentUserId = GetCurrentUserId(claimsPrincipal);
            if (currentUserId != exist.CreatedById)
            {
                throw new UnauthorizedAccessException();
            }

            var farm = new FarmModifyRequest
            {
                Id = criterias.Id,
                CreatedById = currentUserId,
                UpdatedById = currentUserId,
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
                    Id = x.PictureId.GetValueOrDefault()
                });
            }

            await _farmAppService.UpdateAsync(farm);
            return new FarmIdResultModel
            {
                Id = farm.Id
            };
        }

        public async Task<FarmPageListModel> GetUserFarmsAsync(ClaimsPrincipal claimsPrincipal, FarmFilterModel criterias)
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

            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var userId = await _userManager.DecryptUserIdAsync(criterias.UserIdentityId);
            var filterRequest = new FarmFilter
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize.HasValue && criterias.PageSize < _pagerOptions.PageSize ? criterias.PageSize.Value : _pagerOptions.PageSize,
                Keyword = criterias.Search,
                CreatedById = userId,
                CanGetInactived = currentUserId == userId
            };

            try
            {
                var farmPageList = await _farmAppService.GetAsync(filterRequest);
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
                PageSize = criterias.PageSize.HasValue && criterias.PageSize < _pagerOptions.PageSize ? criterias.PageSize.Value : _pagerOptions.PageSize,
                Keyword = criterias.Search
            };

            if (!string.IsNullOrEmpty(criterias.ExclusiveUserIdentityId))
            {
                filterRequest.ExclusiveUserId = await _userManager.DecryptUserIdAsync(criterias.ExclusiveUserIdentityId);
            }

            try
            {
                var farmPageList = await _farmAppService.GetAsync(filterRequest);
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

        public async Task<FarmModel> GetFarmAsync(ClaimsPrincipal claimsPrincipal, FarmIdFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FarmIdFilterModel();
            }

            if (criterias.Id <= 0)
            {
                throw new ArgumentNullException(nameof(criterias.Id));
            }

            try
            {
                var farmResult = await _farmAppService.FindDetailAsync(new IdRequestFilter<long>
                {
                    Id = criterias.Id,
                    CanGetInactived = true
                });

                var currentUserId = GetCurrentUserId(claimsPrincipal);
                if (currentUserId != farmResult.CreatedById)
                {
                    throw new UnauthorizedAccessException();
                }
                var farm = await MapFarmResultToModelAsync(farmResult);
                return farm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteFarmAsync(ClaimsPrincipal claimsPrincipal, FarmIdFilterModel criterias)
        {
            try
            {
                if (criterias.Id <= 0)
                {
                    throw new ArgumentNullException(nameof(criterias.Id));
                }

                var exist = await _farmAppService.FindAsync(new IdRequestFilter<long>
                {
                    Id = criterias.Id,
                    CanGetInactived = true
                });

                if (exist == null)
                {
                    return false;
                }

                var currentUserId = GetCurrentUserId(claimsPrincipal);
                if (currentUserId != exist.CreatedById)
                {
                    throw new UnauthorizedAccessException();
                }

                return await _farmAppService.SoftDeleteAsync(new FarmModifyRequest
                {
                    UpdatedById = currentUserId,
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
                CreatedByPhotoId = farmResult.CreatedByPhotoId,
                Pictures = farmResult.Pictures.Select(y => new PictureResultModel()
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
                CreatedByPhotoId = x.CreatedByPhotoId,
                Pictures = x.Pictures.Select(y => new PictureResultModel()
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
