using Camino.Data.Contracts;

namespace Camino.Service.Business.Setup.Contracts
{
    public interface ISeedDataBusiness
    {
        void CreateDatabase(IBaseDataProvider dataProvider);
        void CreateDataByScript(IBaseDataProvider dataProvider, string sql);
    }
}
