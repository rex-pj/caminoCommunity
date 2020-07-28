using Camino.Core.Constants;
using Camino.Framework.Models;
using Camino.Framework.Providers.Contracts;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Camino.Framework.Providers.Implementation
{
    public class SetupProvider : ISetupProvider
    {
        private readonly IFileProvider _fileProvider;
        private readonly SetupSettings _setupSettings;
        
        public SetupProvider(IFileProvider fileProvider, SetupSettings setupSettings)
        {
            _fileProvider = fileProvider;
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
            filePath ??= SetupSettingsConst.FilePath;

            try
            {
                _setupSettings.HasSetupDatabase = true;
                SaveSettings(_setupSettings, filePath);
            }
            catch (Exception)
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

            filePath ??= SetupSettingsConst.FilePath;
            if (!_fileProvider.FileExists(filePath))
            {
                var installSettings = new SetupSettings()
                {
                    IsInitialized = true,
                    SetupUrl = SetupSettingsConst.SetupUrl,
                    CreateIdentityPath = SetupSettingsConst.CreateIdentityDbPath,
                    PrepareIdentityDataPath = SetupSettingsConst.PrepareIdentityDataPath,
                    CreateContentDbPath = SetupSettingsConst.CreateContentDbPath,
                    PrepareContentDataPath = SetupSettingsConst.PrepareContentDataPath
                };

                SaveSettings(installSettings, filePath);
                _setupSettings.IsInitialized = true;
                return installSettings;
            }

            var text = _fileProvider.ReadText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
            {
                return new SetupSettings();
            }

            var installationSettings = JsonConvert.DeserializeObject<SetupSettings>(text);
            _setupSettings.HasSetupDatabase = installationSettings.HasSetupDatabase;
            return installationSettings;
        }

        public void DeleteSettings(string filePath = null)
        {
            filePath ??= SetupSettingsConst.FilePath;
            _fileProvider.DeleteFile(filePath);
        }

        public void SaveSettings(SetupSettings settings, string filePath)
        {
            _fileProvider.CreateFile(filePath);

            var text = JsonConvert.SerializeObject(settings, Formatting.Indented);
            _fileProvider.WriteText(filePath, text, Encoding.UTF8);
        }
    }
}
