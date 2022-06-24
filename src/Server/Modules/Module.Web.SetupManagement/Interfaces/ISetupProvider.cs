using Module.Web.SetupManagement;

namespace Camino.Core.Contracts.Providers
{
    public interface ISetupProvider
    {
        void SetDatabaseHasBeenSetup(string filePath = null);
        void SetDataHasBeenSeeded(string filePath = null);
        SetupSettings LoadSettings(string filePath = null);
        void DeleteSetupSettings(string filePath = null);
        string LoadFileText(string relativePath);
        bool HasDatabaseSetup();
        bool HasDataSeeded();
        bool HasInitializedSetup();
    }
}
