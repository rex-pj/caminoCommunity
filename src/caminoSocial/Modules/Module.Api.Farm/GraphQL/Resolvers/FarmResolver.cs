using Camino.Core.Models;
using Camino.Framework.GraphQL.Resolvers;
using Camino.Framework.Models;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Contracts.Core;
using Camino.IdentityManager.Models;
using Camino.Service.Business.Farms.Contracts;
using Camino.Service.Projections.Farm;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Media;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using Module.Api.Farm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Api.Farm.GraphQL.Resolvers
{
    public class FarmResolver : BaseResolver, IFarmResolver
    {
        private readonly IFarmBusiness _farmBusiness;
        private readonly IUserManager<ApplicationUser> _userManager;

        public FarmResolver(SessionState sessionState, IFarmBusiness farmBusiness, IUserManager<ApplicationUser> userManager)
            : base(sessionState)
        {
            _farmBusiness = farmBusiness;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ISelectOption>> SelectFarmsAsync(SelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new SelectFilterModel();
            }

            var farms = await _farmBusiness.SearchAsync(criterias.Query);
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
        public async Task<FarmModel> CreateFarmAsync(FarmModel criterias)
        {
            var farm = new FarmProjection()
            {
                CreatedById = CurrentUser.Id,
                UpdatedById = CurrentUser.Id,
                Description = criterias.Description,
                Name = criterias.Name,
                FarmTypeId = criterias.FarmTypeId,
                Address = criterias.Address,
                Pictures = criterias.Thumbnails.Select(x => new PictureRequestProjection()
                {
                    Base64Data = x.Base64Data,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                }),
            };

            var id = await _farmBusiness.CreateAsync(farm);
            criterias.Id = id;
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
                var farmPageList = await _farmBusiness.GetAsync(filterRequest);
                var farms = await MapFarmsProjectionToModelAsync(farmPageList.Collections);

                var farmPage = new FarmPageListModel(farms)
                {
                    Filter = criterias,
                    TotalPage = farmPageList.TotalPage,
                    TotalResult = farmPageList.TotalResult
                };

                return farmPage;
            }
            catch (Exception e)
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

            try
            {
                var farmPageList = await _farmBusiness.GetAsync(filterRequest);
                var farms = await MapFarmsProjectionToModelAsync(farmPageList.Collections);

                var farmPage = new FarmPageListModel(farms)
                {
                    Filter = criterias,
                    TotalPage = farmPageList.TotalPage,
                    TotalResult = farmPageList.TotalResult
                };

                return farmPage;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task<IList<FarmModel>> MapFarmsProjectionToModelAsync(IEnumerable<FarmProjection> farmProjections)
        {
            var farms = farmProjections.Select(x => new FarmModel()
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
                Thumbnails = x.Pictures.Select(y => new PictureRequestModel()
                {
                    Id = y.Id
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
