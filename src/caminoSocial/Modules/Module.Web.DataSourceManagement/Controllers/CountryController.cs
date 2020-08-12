using Camino.Business.Contracts;
using Camino.Core.Constants;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Module.Web.DataSourceManagement.Controllers
{
    public class CountryController : BaseAuthController
    {
        private readonly ICountryBusiness _countryBusiness;
        public CountryController(IHttpContextAccessor httpContextAccessor, ICountryBusiness countryBusiness) : base(httpContextAccessor)
        {
            _countryBusiness = countryBusiness;
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadCountry)]
        public IActionResult Search(string q)
        {
            var countries = _countryBusiness.Search(q);
            if (countries == null || !countries.Any())
            {
                return Json(new
                {
                    Items = new List<Select2ItemModel>()
                });
            }

            var userModels = countries
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return Json(userModels);
        }
    }
}