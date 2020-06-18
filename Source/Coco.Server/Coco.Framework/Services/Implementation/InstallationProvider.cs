using Coco.Common.Const;
using Coco.Framework.Models;
using Coco.Framework.Services.Contracts;
using Newtonsoft.Json;
using System;
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
            if (!IsDatabaseInstalled)
            {
                IsDatabaseInstalled = LoadSettings().IsDatabaseInstalled;
            }

            if (!IsInitialized)
            {
                IsInitialized = LoadSettings().IsInitialized;
            }
        }

        public bool IsDatabaseInstalled { get; }
        public bool IsInitialized { get; }

        public void SetDatabaseInstalled(string filePath = null)
        {
            filePath = filePath == null ? InstallationSettingsConst.FilePath : filePath;

            try
            {
                _installationSettings.IsDatabaseInstalled = true;
                SaveSettings(_installationSettings, filePath);
            }
            catch (Exception e)
            {
                _installationSettings.IsDatabaseInstalled = false;
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
                    IsInitialized = true
                };

                SaveSettings(installSettings, filePath);
                _installationSettings.IsInitialized = true;
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
