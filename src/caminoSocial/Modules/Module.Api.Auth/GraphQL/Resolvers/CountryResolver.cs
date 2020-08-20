using Module.Api.Auth.Models;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using Camino.Service.Business.Identities.Contracts;

namespace Module.Api.Auth.GraphQL.Resolvers
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
