﻿using Camino.Core.Contracts.DependencyInjection;
using Camino.Core.Contracts.Repositories.Setup;
using Camino.Core.Contracts.Services.Setup;
using Camino.Shared.Requests.Setup;
using System.Threading.Tasks;

namespace Camino.Services.Setup
{
    public class DataSeedService : IDataSeedService, IScopedDependency
    {
        private readonly IDbCreationRepository _dbCreationRepository;
        private readonly IDataSeedRepository _dataSeedRepository;
        public DataSeedService(IDbCreationRepository dbCreationRepository, IDataSeedRepository dataSeedRepository)
        {
            _dbCreationRepository = dbCreationRepository;
            _dataSeedRepository = dataSeedRepository;
        }

        public async Task CreateDatabaseAsync()
        {
            await _dbCreationRepository.CreateDatabaseAsync();
        }

        public async Task SeedDataAsync(SetupRequest setupRequest)
        {
            await _dataSeedRepository.SeedDataAsync(setupRequest);
        }
    }
}
