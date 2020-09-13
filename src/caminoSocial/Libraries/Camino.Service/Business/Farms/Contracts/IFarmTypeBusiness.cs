using Camino.DAL.Entities;
using Camino.Service.Projections.Farm;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Camino.Service.Business.Farms.Contracts
{
    public interface IFarmTypeBusiness
    {
        FarmTypeProjection Find(long id);
        FarmTypeProjection FindByName(string name);
        Task<BasePageList<FarmTypeProjection>> GetAsync(FarmTypeFilter filter);
        List<FarmTypeProjection> Get(Expression<Func<FarmType, bool>> filter);
        IList<FarmTypeProjection> Search(string search = "", int page = 1, int pageSize = 10);
        int Add(FarmTypeProjection farmType);
        FarmTypeProjection Update(FarmTypeProjection farmType);
    }
}
