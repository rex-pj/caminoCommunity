using Camino.Shared.Requests.Setup;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Setup
{
    public interface IDataSeedRepository
    {
        Task SeedDataAsync(SetupRequest setupRequest);
    }
}
