using Module.Setup.WebAdmin.Dtos;
using System.Threading.Tasks;

namespace Module.Setup.WebAdmin.AppServices
{
    public interface IDataSeedAppService
    {
        Task CreateDatabaseAsync();
        Task SeedDataAsync(SetupRequest setupRequest);
    }
}
