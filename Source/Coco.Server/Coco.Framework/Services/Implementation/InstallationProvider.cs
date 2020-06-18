using Coco.Common.Const;
using Coco.Framework.Models;
using Coco.Framework.Services.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace Coco.Framework.Services.Implementation
{
    public class InstallationProvider : IInstallationProvider
    {
        private readonly IFileAccessor _fileAccessor;
        private readonly InstallationSettings _installationSettings;
        public InstallationProvider(IFileAccessor fileAccessor, InstallationSettings installationSettings)
        {
            _fileAccessor = fileAccessor;
            _installationSettings = installationSettings;
        }

        private bool _isDatabaseInstalled;

        public bool IsDatabaseInstalled
        {
            get
            {
                if (!_isDatabaseInstalled)
                {
                    _isDatabaseInstalled = LoadSettings().IsDatabaseInstalled;
                }

                return _isDatabaseInstalled;
            }
        }

        public InstallationSettings LoadSettings(string filePath = null)
        {
            if (_installationSettings != null && _installationSettings.IsDatabaseInstalled)
            {
                return _installationSettings;
            }

            filePath = filePath == null ? InstallationSettingsConst.FilePath : filePath;
            if (!_fileAccessor.FileExists(filePath))
            {
                var installSettings = new InstallationSettings()
                {
                    IsDatabaseInstalled = true
                };

                SaveSettings(installSettings, filePath);
                _installationSettings.IsDatabaseInstalled = true;
                return installSettings;
            }

            var text = _fileAccessor.ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
            {
                return new InstallationSettings();
            }

            var installationSettings = JsonConvert.DeserializeObject<InstallationSettings>(text);
            _installationSettings.IsDatabaseInstalled = installationSettings.IsDatabaseInstalled;
            return installationSettings;
        }

        public void SaveSettings(InstallationSettings settings, string filePath)
        {
            _fileAccessor.CreateFile(filePath);

            var text = JsonConvert.SerializeObject(settings, Formatting.Indented);
            _fileAccessor.WriteAllText(filePath, text, Encoding.UTF8);
        }
    }
}
