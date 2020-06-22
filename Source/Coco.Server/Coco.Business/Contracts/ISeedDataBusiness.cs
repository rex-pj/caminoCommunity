using Coco.Entities.Dtos.General;

namespace Coco.Business.Contracts
{
    public interface ISeedDataBusiness
    {
        bool IsDatabaseExist();
        void SeedingIdentityDb(InstallationDto installationDto);
    }
}
