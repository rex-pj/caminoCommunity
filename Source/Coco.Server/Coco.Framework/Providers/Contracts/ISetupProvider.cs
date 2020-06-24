using Coco.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Framework.Providers.Contracts
{
    public interface ISetupProvider
    {
        void SetDatabaseHasBeenSetup(string filePath = null);
        SetupSettings LoadSettings(string filePath = null);
        bool HasSetupDatabase { get; }
        bool IsInitialized { get; }
    }
}
