using Camino.Service.Projections.Request;
using System.Threading.Tasks;

namespace Camino.Service.Business.Setup.Contracts
{
    public interface IContentDataSetupBusiness
    {
        void SeedingContentDb(string sql);
        Task PrepareContentDataAsync(SetupRequest installationRequest);
    }
}
