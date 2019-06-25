using Coco.Entities.Domain.Dbo;
using System.Collections.Generic;

namespace Coco.Business.Contracts
{
    public interface ICountryBusiness
    {
        List<Country> GetAll();
    }
}
