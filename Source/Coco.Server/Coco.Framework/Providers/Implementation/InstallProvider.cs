using Coco.Common.Const;
using Coco.Framework.Models;
using Coco.Framework.Providers.Contracts;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Coco.Framework.Providers.Implementation
{
    public class InstallProvider : IInstallProvider
    {
        private readonly IFileProvider _fileAccessor;
        private readonly InstallSettings _installationSettings;
        public InstallProvider(IFileProvider fileAccessor, InstallSettings installationSettings)
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

        public InstallSettings LoadSettings(string filePath = null)
        {
            if (_installationSettings != null && _installationSettings.IsDatabaseInstalled)
            {
                return _installationSettings;
            }

            filePath = filePath == null ? InstallationSettingsConst.FilePath : filePath;
            if (!_fileAccessor.FileExists(filePath))
            {
                var installSettings = new InstallSettings()
                {
                    IsInitialized = true
                };

                SaveSettings(installSettings, filePath);
                _installationSettings.IsInitialized = true;
                return installSettings;
            }

            var text = _fileAccessor.ReadText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
            {
                return new InstallSettings();
            }

            var installationSettings = JsonConvert.DeserializeObject<InstallSettings>(text);
            _installationSettings.IsDatabaseInstalled = installationSettings.IsDatabaseInstalled;
            return installationSettings;
        }

        public void SaveSettings(InstallSettings settings, string filePath)
        {
            _fileAccessor.CreateFile(filePath);

            var text = JsonConvert.SerializeObject(settings, Formatting.Indented);
            _fileAccessor.WriteText(filePath, text, Encoding.UTF8);
        }
    }
}
