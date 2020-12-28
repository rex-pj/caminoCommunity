using Camino.Service.Projections.Request;
using System.Threading.Tasks;

namespace Camino.Service.Business.Setup.Contracts
{
    public interface IIdentityDataSetupBusiness
    {
        bool IsIdentityDatabaseExist();
        void SeedingIdentityDb(string sql);
        Task PrepareIdentityDataAsync(SetupRequest installationRequest);
    }
}
