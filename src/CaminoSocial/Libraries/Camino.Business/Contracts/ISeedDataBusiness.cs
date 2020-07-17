using Camino.Business.Dtos.General;

namespace Camino.Business.Contracts
{
    public interface ISeedDataBusiness
    {

        bool IsIdentityDatabaseExist();
        void SeedingIdentityDb(string sql);
        void SeedingContentDb(string sql);
        void PrepareIdentityData(SetupDto installationDto);
    }
}
