using Api.Identity.Models;
using System.Collections.Generic;

namespace Api.Identity.Resolvers.Contracts
{
    public interface ICountryResolver
    {
        IEnumerable<CountryModel> GetAll();
    }
}
