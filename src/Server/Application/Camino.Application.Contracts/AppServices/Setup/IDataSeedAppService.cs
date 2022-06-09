using Camino.Application.Contracts.AppServices.Setup.Dtos;

namespace Camino.Application.Contracts.AppServices.Setup
{
    public interface IDataSeedAppService
    {
        Task CreateDatabaseAsync();
        Task SeedDataAsync(SetupRequest setupRequest);
    }
}
