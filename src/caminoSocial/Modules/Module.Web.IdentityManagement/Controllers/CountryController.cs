using AutoMapper;
using Camino.Business.Contracts;
using Camino.Business.Dtos.General;
using Camino.Business.Dtos.Identity;
using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Helpers.Contracts;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.IdentityManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Web.IdentityManagement.Controllers
{
    public class CountryController : BaseAuthController
    {
        private readonly ICountryBusiness _countryBusiness;
        private readonly IHttpHelper _httpHelper;

        public CountryController(IHttpContextAccessor httpContextAccessor, ICountryBusiness countryBusiness,
            IHttpHelper httpHelper) : base(httpContextAccessor)
        {
            _countryBusiness = countryBusiness;
            _httpHelper = httpHelper;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadCountry)]
        [LoadResultAuthorizations("Country", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(CountryFilterModel filter)
        {
            var filterDto = new CountryFilterDto()
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search
            };

            var countryPageList = await _countryBusiness.GetAsync(filterDto);
            var countries = countryPageList.Collections.Select(x => new CountryModel() { 
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
                var country = _countryBusiness.Find(id);
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
            var country = _countryBusiness.Find(id);
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
        public IActionResult Create(CountryModel model)
        {
            var country = new CountryDto()
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code
            };

            var exist = _countryBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var id = _countryBusiness.Add(country);

            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateCountry)]
        public IActionResult Update(CountryModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _countryBusiness.Find(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var country = new CountryDto()
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code
            };

            _countryBusiness.Update(country);
            return RedirectToAction("Detail", new { id = country.Id });
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