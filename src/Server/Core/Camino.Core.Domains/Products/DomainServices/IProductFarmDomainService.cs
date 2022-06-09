using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Domains.Products.DomainServices
{
    public interface IProductFarmDomainService
    {
        Task<bool> UpdateProductFarmRelationsAsync(long productId, IList<long> farmIds,
            long modifiedById, bool needSaveChanges = false);
    }
}
