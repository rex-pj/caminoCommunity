using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Setup
{
    public interface IDbCreationRepository
    {
        bool IsDatabaseExist();
        Task CreateDatabaseAsync();
    }
}
