using Api.Identity.Models;
using Api.Identity.Resolvers.Contracts;
using Coco.Business.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Api.Identity.Resolvers
{
    public class CountryResolver : ICountryResolver
    {
        private readonly ICountryBusiness _countryBusiness;
        public CountryResolver(ICountryBusiness countryBusiness)
        {
            _countryBusiness = countryBusiness;
        }

        public IEnumerable<CountryModel> GetAll()
        {
            var countries = _countryBusiness.GetAll();
            if (countries == null || !countries.Any())
            {
                return new List<CountryModel>();
            }

            return countries.Select(x => new CountryModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }
    }
}
