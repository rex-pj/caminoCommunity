using Coco.Common.Const;
using Coco.Framework.Models;
using Coco.Framework.Providers.Contracts;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Coco.Framework.Providers.Implementation
{
    public class SetupProvider : ISetupProvider
    {
        private readonly IFileProvider _fileAccessor;
        private readonly SetupSettings _setupSettings;
        public SetupProvider(IFileProvider fileAccessor, SetupSettings setupSettings)
        {
            _fileAccessor = fileAccessor;
            _setupSettings = setupSettings;
            if (!HasSetupDatabase)
            {
                HasSetupDatabase = LoadSettings().HasSetupDatabase;
            }

            if (!IsInitialized)
            {
                IsInitialized = LoadSettings().IsInitialized;
            }
        }

        public bool HasSetupDatabase { get; }
        public bool IsInitialized { get; }

        public void SetDatabaseHasBeenSetup(string filePath = null)
        {
            filePath = filePath == null ? SetupSettingsConst.FilePath : filePath;

            try
            {
                _setupSettings.HasSetupDatabase = true;
                SaveSettings(_setupSettings, filePath);
            }
            catch (Exception e)
            {
                _setupSettings.HasSetupDatabase = false;
            }
        }

        public SetupSettings LoadSettings(string filePath = null)
        {
            if (_setupSettings != null && _setupSettings.HasSetupDatabase)
            {
                return _setupSettings;
            }

            filePath = filePath == null ? SetupSettingsConst.FilePath : filePath;
            if (!_fileAccessor.FileExists(filePath))
            {
                var installSettings = new SetupSettings()
                {
                    IsInitialized = true,
                    SetupUrl = SetupSettingsConst.SetupUrl,
                    CreateIdentityPath = SetupSettingsConst.CreateIdentityPath
                };

                SaveSettings(installSettings, filePath);
                _setupSettings.IsInitialized = true;
                return installSettings;
            }

            var text = _fileAccessor.ReadText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
            {
                return new SetupSettings();
            }

            var installationSettings = JsonConvert.DeserializeObject<SetupSettings>(text);
            _setupSettings.HasSetupDatabase = installationSettings.HasSetupDatabase;
            return installationSettings;
        }

        public void SaveSettings(SetupSettings settings, string filePath)
        {
            _fileAccessor.CreateFile(filePath);

            var text = JsonConvert.SerializeObject(settings, Formatting.Indented);
            _fileAccessor.WriteText(filePath, text, Encoding.UTF8);
        }
    }
}
