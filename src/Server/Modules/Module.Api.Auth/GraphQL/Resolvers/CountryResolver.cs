using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Identifiers;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class CountryResolver : ICountryResolver
    {
        private readonly ICountryAppService _countryAppService;
        public CountryResolver(ICountryAppService countryAppService)
        {
            _countryAppService = countryAppService;
        }

        public IEnumerable<SelectOption> GetSelections()
        {
            var countries = _countryAppService.Get();
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
