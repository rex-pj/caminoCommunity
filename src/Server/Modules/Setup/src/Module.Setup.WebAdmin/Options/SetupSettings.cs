namespace Module.Setup.WebAdmin
{
    public class SetupSettings
    {
        public bool HasSetupDatabase { get; set; }
        public bool HasSeededData { get; set; }
        public bool IsInitialized { get; set; }
        public string SeedDataJsonFilePath { get; set; }
    }
}
