using Coco.Entities.Domain.Identity;
using System.Collections.Generic;

namespace Coco.Business.Contracts
{
    public interface ICountryBusiness
    {
        List<Country> GetAll();
    }
}
