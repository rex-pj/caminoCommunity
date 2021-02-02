using Camino.Service.Projections.Filters;
using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Helpers.Contracts;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Products.Contracts;
using Camino.Service.Projections.Product;
using Camino.Service.Business.Farms.Contracts;
using Camino.Service.Projections.Farm;

namespace Module.Web.ProductManagement.Controllers
{
    public class ProductFarmController : BaseAuthController
    {
        private readonly IFarmBusiness _farmBusiness;
        private readonly IHttpHelper _httpHelper;

        public ProductFarmController(IFarmBusiness farmBusiness,
            IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _farmBusiness = farmBusiness;
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
            var farms = await _farmBusiness.SelectAsync(filter);
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