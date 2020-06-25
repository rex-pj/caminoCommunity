using Coco.Entities.Dtos.General;

namespace Coco.Business.Contracts
{
    public interface ISeedDataBusiness
    {
        bool IsIdentityDatabaseExist();
        void SeedingIdentityDb(SetupDto installationDto, string sql);
        void SeedingContentDb(SetupDto installationDto, string sql);
    }
}
