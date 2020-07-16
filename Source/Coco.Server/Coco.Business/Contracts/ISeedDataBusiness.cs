using Coco.Core.Dtos.General;

namespace Coco.Business.Contracts
{
    public interface ISeedDataBusiness
    {

        bool IsIdentityDatabaseExist();
        void SeedingIdentityDb(string sql);
        void SeedingContentDb(string sql);
        void PrepareIdentityData(Setup installationDto);
    }
}
