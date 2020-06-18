using Coco.Framework.Models;

namespace Coco.Framework.Services.Contracts
{
    public interface IInstallationProvider
    {
        InstallationSettings LoadSettings(string filePath = null);
        bool IsDatabaseInstalled { get; }
    }
}
