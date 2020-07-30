using Camino.Business.Dtos.General;
using System.Threading.Tasks;

namespace Camino.Business.Contracts
{
    public interface ISeedDataBusiness
    {

        bool IsIdentityDatabaseExist();
        void SeedingIdentityDb(string sql);
        void SeedingContentDb(string sql);
        Task PrepareIdentityDataAsync(SetupDto installationDto);
        Task PrepareContentDataAsync(SetupDto installationDto);
    }
}
