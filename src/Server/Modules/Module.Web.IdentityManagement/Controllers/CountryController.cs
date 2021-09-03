using Camino.Shared.Requests.Filters;
using Camino.Shared.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Core.Contracts.Helpers;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.IdentityManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Requests.Identifiers;
using Camino.Core.Contracts.Services.Identities;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Options;
using Camino.Infrastructure.Commons.Constants;

namespace Module.Web.IdentityManagement.Controllers
{
    public class CountryController : BaseAuthController
    {
        private readonly ICountryService _countryService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public CountryController(IHttpContextAccessor httpContextAccessor, ICountryService countryService,
            IHttpHelper httpHelper, IOptions<PagerOptions> pagerOptions) : base(httpContextAccessor)
        {
            _countryService = countryService;
            _httpHelper = httpHelper;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadCountry)]
        [LoadResultAuthorizations("Country", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(CountryFilterModel filter)
        {
            var filterRequest = new CountryFilter()
            {
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search
            };

            var countryPageList = await _countryService.GetAsync(filterRequest);
            var countries = countryPageList.Collections.Select(x => new CountryModel()
            {
                Code = x.Code,
                Id = x.Id,
                Name = x.Name
            });

            var countryPage = new PageListModel<CountryModel>(countries)
            {
                Filter = filter,
                TotalPage = countryPageList.TotalPage,
                TotalResult = countryPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_CountryTable", countryPage);
            }

            return View(countryPage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadCountry)]
        [LoadResultAuthorizations("Country", PolicyMethod.CanUpdate)]
        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var country = _countryService.Find(id);
                if (country == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new CountryModel()
                {
                    Id = country.Id,
                    Name = country.Name,
                    Code = country.Code
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateCountry)]
        public IActionResult Create()
        {
            var model = new CountryModel();
            return View(model);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateCountry)]
        public IActionResult Update(int id)
        {
            var country = _countryService.Find(id);
            var model = new CountryModel()
            {
                Id = country.Id,
                Name = country.Name,
                Code = country.Code
            };

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateCountry)]
        public async Task<IActionResult> Create(CountryModel model)
        {
            var country = new CountryModifyRequest()
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code
            };

            var exist = _countryService.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var id = await _countryService.CreateAsync(country);

            return RedirectToAction(nameof(Detail), new { id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateCountry)]
        public IActionResult Update(CountryModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _countryService.Find(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var country = new CountryModifyRequest()
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code
            };

            _countryService.UpdateAsync(country);
            return RedirectToAction(nameof(Detail), new { id = country.Id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadCountry)]
        public IActionResult Search(string q)
        {
            var countries = _countryService.Search(new BaseFilter { 
                Page = _defaultPageSelection,
                Keyword = q,
                PageSize = _pagerOptions.PageSize
            });
            if (countries == null || !countries.Any())
            {
                return Json(new
                {
                    Items = new List<Select2ItemModel>()
                });
            }

            var countrySelections = countries
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return Json(countrySelections);
        }
    }
}