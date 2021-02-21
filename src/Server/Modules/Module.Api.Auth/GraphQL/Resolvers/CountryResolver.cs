using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using Camino.Core.Contracts.Services.Identities;
using Camino.Shared.General;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class CountryResolver : ICountryResolver
    {
        private readonly ICountryService _countryService;
        public CountryResolver(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public IEnumerable<SelectOption> GetSelections()
        {
            var countries = _countryService.Get();
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
