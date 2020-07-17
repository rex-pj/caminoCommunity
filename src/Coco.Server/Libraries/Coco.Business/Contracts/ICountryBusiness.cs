using Coco.Core.Entities.Identity;
using System.Collections.Generic;

namespace Coco.Business.Contracts
{
    public interface ICountryBusiness
    {
        List<Country> GetAll();
    }
}
