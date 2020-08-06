using Camino.Framework.Models.Settings;

namespace Camino.Framework.Providers.Contracts
{
    public interface ISetupProvider
    {
        void SetDatabaseHasBeenSetup(string filePath = null);
        SetupSettings LoadSettings(string filePath = null);
        bool HasSetupDatabase { get; }
        bool IsInitialized { get; }
        void DeleteSettings(string filePath = null);
    }
}
