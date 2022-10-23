using Camino.Infrastructure.Identity.Attributes;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Infrastructure.AspNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Farms;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Constants;
using Camino.Application.Contracts;

namespace Module.Web.ProductManagement.Controllers
{
    public class ProductFarmController : BaseAuthController
    {
        private readonly IFarmAppService _farmAppService;
        private readonly PagerOptions _pagerOptions;

        public ProductFarmController(IFarmAppService farmAppService, IHttpContextAccessor httpContextAccessor, IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _farmAppService = farmAppService;
            _pagerOptions = pagerOptions.Value;
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadFarm)]
        public async Task<IActionResult> Search(string q, string currentId = null)
        {
            long[] currentIds = null;
            if (!string.IsNullOrEmpty(currentId))
            {
                currentIds = currentId.Split(',').Select(x => long.Parse(x)).ToArray();
            }

            var filter = new SelectFilter()
            {
                CurrentIds = currentIds,
                Keyword = q
            };
            var farms = await _farmAppService.SelectAsync(filter, 1, _pagerOptions.PageSize);
            if (farms == null || !farms.Any())
            {
                return Json(new List<Select2ItemModel>());
            }

            var farmSeletions = farms
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return Json(farmSeletions);
        }
    }
}