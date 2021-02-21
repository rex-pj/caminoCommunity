using Camino.Shared.Requests.Setup;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Setup
{
    public interface IDataSeedService
    {
        Task CreateDatabaseAsync(string sql);
        Task SeedDataAsync(SetupRequest setupRequest);
    }
}
