using Module.Api.Auth.Models;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using Camino.Service.Business.Identities.Contracts;
using Camino.Core.Models;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class CountryResolver : ICountryResolver
    {
        private readonly ICountryBusiness _countryBusiness;
        public CountryResolver(ICountryBusiness countryBusiness)
        {
            _countryBusiness = countryBusiness;
        }

        public IEnumerable<SelectOption> GetSelections()
        {
            var countries = _countryBusiness.Get();
            if (countries == null || !countries.Any())
            {
                return new List<SelectOption>();
            }

            return countries.Select(x => new SelectOption()
            {
                Id = x.Id.ToString(),
                Text = x.Name
            }).ToList();
        }
    }
}
