using Coco.Api.Auth.Models;
using System.Collections.Generic;

namespace  Coco.Api.Auth.Resolvers.Contracts
{
    public interface ICountryResolver
    {
        IEnumerable<CountryModel> GetAll();
    }
}
