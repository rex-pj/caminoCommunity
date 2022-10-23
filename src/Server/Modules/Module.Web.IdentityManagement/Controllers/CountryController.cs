using Camino.Shared.Enums;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Infrastructure.AspNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.IdentityManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Identifiers;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Constants;
using Camino.Application.Contracts.AppServices.Identifiers.Dtos;
using Camino.Application.Contracts;
using Camino.Infrastructure.Identity.Attributes;

namespace Module.Web.IdentityManagement.Controllers
{
    public class CountryController : BaseAuthController
    {
        private readonly ICountryAppService _countryAppService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public CountryController(IHttpContextAccessor httpContextAccessor, ICountryAppService countryAppService,
            IHttpHelper httpHelper, IOptions<PagerOptions> pagerOptions) : base(httpContextAccessor)
        {
            _countryAppService = countryAppService;
            _httpHelper = httpHelper;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadCountry)]
        [PopulatePermissions("Country", PolicyMethods.CanCreate, PolicyMethods.CanUpdate, PolicyMethods.CanDelete)]
        public async Task<IActionResult> Index(CountryFilterModel filter)
        {
            var filterRequest = new CountryFilter()
            {
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search
            };

            var countryPageList = await _countryAppService.GetAsync(filterRequest);
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

        [ApplicationAuthorize(AuthorizePolicies.CanReadCountry)]
        [PopulatePermissions("Country", PolicyMethods.CanUpdate)]
        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var country = _countryAppService.Find(id);
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

        [ApplicationAuthorize(AuthorizePolicies.CanCreateCountry)]
        public IActionResult Create()
        {
            var model = new CountryModel();
            return View(model);
        }

        [ApplicationAuthorize(AuthorizePolicies.CanUpdateCountry)]
        public IActionResult Update(int id)
        {
            var country = _countryAppService.Find(id);
            var model = new CountryModel()
            {
                Id = country.Id,
                Name = country.Name,
                Code = country.Code
            };

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanCreateCountry)]
        public async Task<IActionResult> Create(CountryModel model)
        {
            var country = new CountryModifyRequest()
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code
            };

            var exist = _countryAppService.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var id = await _countryAppService.CreateAsync(country);

            return RedirectToAction(nameof(Detail), new { id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateCountry)]
        public IActionResult Update(CountryModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _countryAppService.Find(model.Id);
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

            _countryAppService.UpdateAsync(country);
            return RedirectToAction(nameof(Detail), new { id = country.Id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadCountry)]
        public IActionResult Search(string q)
        {
            var countries = _countryAppService.Search(new BaseFilter { 
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