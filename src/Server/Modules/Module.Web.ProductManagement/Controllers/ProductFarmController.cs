using Camino.Shared.Requests.Filters;
using Camino.Core.Constants;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Farms;

namespace Module.Web.ProductManagement.Controllers
{
    public class ProductFarmController : BaseAuthController
    {
        private readonly IFarmService _farmService;

        public ProductFarmController(IFarmService farmService, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _farmService = farmService;
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductCategory)]
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
                Search = q
            };
            var farms = await _farmService.SelectAsync(filter);
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