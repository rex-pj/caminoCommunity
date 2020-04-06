using Api.Public.Models;
using System.Collections.Generic;

namespace Api.Public.Resolvers.Contracts
{
    public interface ICountryResolver
    {
        IEnumerable<CountryModel> GetAll();
    }
}
