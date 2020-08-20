using Camino.Service.Data.Request;
using System.Threading.Tasks;

namespace Camino.Service.Business.Setup.Contracts
{
    public interface ISetupBusiness
    {

        bool IsIdentityDatabaseExist();
        void SeedingIdentityDb(string sql);
        void SeedingContentDb(string sql);
        Task PrepareIdentityDataAsync(SetupRequest installationDto);
        Task PrepareContentDataAsync(SetupRequest installationDto);
    }
}
