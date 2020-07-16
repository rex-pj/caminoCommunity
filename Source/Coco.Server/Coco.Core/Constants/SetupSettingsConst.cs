namespace Coco.Core.Constants
{
    public static class SetupSettingsConst
    {
        public const string SetupUrl = "/Setup";
        public const string FilePath = "App_Data\\setupsettings.json";
        public const string CreateIdentityDbPath = "App_Data\\Setup\\01.Create_IdentityDb.sql";
        public const string CreateContentDbPath = "App_Data\\Setup\\02.Create_ContentDb.sql";
        public const string PrepareIdentityDataPath = "App_Data\\Setup\\Identity_Data.json";
        public const string PrepareContentDataPath = "App_Data\\Setup\\Content_Data.json";
    }
}
