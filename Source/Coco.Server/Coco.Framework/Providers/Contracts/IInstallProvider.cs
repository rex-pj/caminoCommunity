using Coco.Framework.Models;

namespace Coco.Framework.Providers.Contracts
{
    public interface IInstallProvider
    {
        void SetDatabaseInstalled(string filePath = null);
        InstallSettings LoadSettings(string filePath = null);
        bool IsDatabaseInstalled { get; }
        bool IsInitialized { get; }
    }
}
