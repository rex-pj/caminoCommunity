using Module.Api.Auth.Models;
using System.Collections.Generic;

namespace  Module.Api.Auth.Resolvers.Contracts
{
    public interface ICountryResolver
    {
        IEnumerable<CountryModel> GetAll();
    }
}
