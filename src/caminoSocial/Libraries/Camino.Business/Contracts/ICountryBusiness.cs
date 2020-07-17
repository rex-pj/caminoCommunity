using Camino.Data.Entities.Identity;
using System.Collections.Generic;

namespace Camino.Business.Contracts
{
    public interface ICountryBusiness
    {
        List<Country> GetAll();
    }
}
