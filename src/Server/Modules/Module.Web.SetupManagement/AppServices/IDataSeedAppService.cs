using Module.Web.SetupManagement.Dtos;
using System.Threading.Tasks;

namespace Module.Web.SetupManagement.AppServices
{
    public interface IDataSeedAppService
    {
        Task CreateDatabaseAsync();
        Task SeedDataAsync(SetupRequest setupRequest);
    }
}
