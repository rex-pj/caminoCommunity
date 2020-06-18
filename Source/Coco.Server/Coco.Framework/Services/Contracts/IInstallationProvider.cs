using Coco.Framework.Models;

namespace Coco.Framework.Services.Contracts
{
    public interface IInstallationProvider
    {
        void SetDatabaseInstalled(string filePath = null);
        InstallationSettings LoadSettings(string filePath = null);
        bool IsDatabaseInstalled { get; }
        bool IsInitialized { get; }
    }
}
