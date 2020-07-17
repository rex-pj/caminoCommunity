using Api.Auth.Models;
using System.Collections.Generic;

namespace Api.Auth.Resolvers.Contracts
{
    public interface ICountryResolver
    {
        IEnumerable<CountryModel> GetAll();
    }
}
