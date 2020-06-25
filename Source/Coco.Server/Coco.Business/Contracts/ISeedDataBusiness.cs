using Coco.Entities.Dtos.General;
using Coco.Entities.Dtos.User;

namespace Coco.Business.Contracts
{
    public interface ISeedDataBusiness
    {
        bool IsIdentityDatabaseExist();
        void SeedingIdentityDb(SetupDto installationDto, string sql);
        void SeedingContentDb(SetupDto installationDto, string sql);
        void CreateInitialUser(UserDto userDto);
        void SeedingIdentityData(SetupDto installationDto, string sql);
    }
}
